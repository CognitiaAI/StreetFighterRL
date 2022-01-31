import numpy as np
import tensorflow as tf

from pipeline.model.cnn_duel_dqn_model import CNNDuelDQNModel
from pipeline.data_provider.basic_data_provider import BasicDataProvider
from pipeline.rewarder.health_rewarder_simple import get_reward

class DoubleDuelDQNTrainer:
    def __init__(self):

        # TODO: Make these paramters readable from config files
        self.__main_model = None
        self.__target_model = None
        self.__data_provider = BasicDataProvider()
        self.eps_min = 0.1
        self.eps_max = 0.7
        self.eps_decay_steps = 1400000
        self.n_steps = 3000000  # total number of training steps
        self.training_start = 64  # start training after 64 game iterations
        self.training_interval = 200  # run a training step every 200 game iterations
        self.save_steps = 1000  # save the model every 1,000 training steps
        self.copy_steps = 2000  # copy online DQN to target DQN every 2000 training steps
        self.discount_rate = 0.95

        self.batch_size = 64
        self.iteration = 0  # game iterations
        self.checkpoint_path = "./model/street_fighter_cnn_dqn.ckpt"
        self.done = True  # env needs to be reset
        self.loss_val = np.infty
        self.game_length = 0
        self.total_max_q = 0
        self.mean_max_q = 0.0
        self.__sess = None
        self.previous_state = None
        self.previous_image = None
        self.previous_action = None
        self.n_outputs = 25
        self.step = 0
        self.main_vars = None
        self.target_vars = None
        self.copy_ops = None
        self.copy_online_to_target = None

    def updateTarget(self):
        self.__sess.run(self.copy_online_to_target)


    def epsilon_greedy(self, q_values, step):
        epsilon = max(self.eps_min, self.eps_max - (self.eps_max - self.eps_min) * step / self.eps_decay_steps)
        if np.random.rand() < epsilon:
            return np.random.randint(self.n_outputs)  # random action
        else:
            return np.argmax(q_values)  # optimal action

    def train_step(self, game_state, screen_shot):

        if self.__sess is None:
            self.__sess = tf.Session()
            self.__main_model = CNNDuelDQNModel('main', self.__sess)
            self.__target_model = CNNDuelDQNModel('target', self.__sess)
            self.main_vars = self.__main_model.trainable_vars_by_name
            self.target_vars = self.__target_model.trainable_vars_by_name
            self.__main_model.sess.run(self.__main_model.init)
            self.__target_model.sess.run(self.__target_model.init)
            self.copy_ops = [target_var.assign(self.main_vars[var_name])
                             for var_name, target_var in self.target_vars.items()]
            self.copy_online_to_target = tf.group(*self.copy_ops)

        # step = self.__model.sess.run(self.__model.global_step)

        if self.previous_state is not None:
            reward = get_reward(self.previous_state, game_state, self.previous_action)
            # Sample memories and use the target DQN to produce the target Q-Value
            batch = self.__data_provider.store_and_retrieve((self.previous_state, self.previous_image, self.previous_action, reward,
                                                             game_state, screen_shot, 1.0 - 0.0), self.batch_size)
            if batch is not None and self.step % self.training_interval == 0:

                print("About to run train step after ", str(self.step), " iterations")
                prev_states, prev_images, prev_actions, rewards, next_states, next_images, continues = batch

                predictions = self.__main_model.sess.run(self.__main_model.predict,
                                                         feed_dict={self.__main_model.state: next_states,\
                                                                      self.__main_model.image: next_images})
                next_q_values = self.__target_model.sess.run(self.__target_model.q_values,
                                                      feed_dict={self.__target_model.state: next_states,
                                                                 self.__target_model.image: next_images})

                max_next_q_values = np.max(next_q_values, axis=1, keepdims=True)

                y_val = (rewards + continues.reshape((-1, 1)) * self.discount_rate * max_next_q_values)
                next_q_values[range(len(next_q_values)), predictions] = y_val.reshape((-1,))
                # Train the online DQN
                _, loss_val = self.__main_model.sess.run([self.__main_model.training_op, self.__main_model.loss], feed_dict={
                    self.__main_model.state: prev_states, self.__main_model.image: prev_images, self.__main_model.action: prev_actions, self.__main_model.y: next_q_values})
                print("Iteration = {}, Loss = {} ".format(self.step, loss_val))

            else:
                print("Step = {}".format(self.step))

        self.previous_state = game_state
        self.previous_image = screen_shot
        q_values = self.__main_model.sess.run(self.__main_model.q_values,
                                              feed_dict={self.__main_model.state: game_state.reshape((1, -1)),
                                                         self.__main_model.image: screen_shot[np.newaxis, :, :, :]})
        action = self.epsilon_greedy(q_values, self.step)
        self.previous_action = action
        self.step += 1

        if self.step % self.copy_steps == 0:
            self.updateTarget()
        return action






