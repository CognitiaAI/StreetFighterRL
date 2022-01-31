from learning.pipeline.input_provider.normalized_one_hot_input_provider import NormalizedOneHotInputProvider
from learning.pipeline.trainer.double_duel_dqn_trainer import DoubleDuelDQNTrainer
from learning.pipeline.model.cnn_duel_dqn_model import CNNDuelDQNModel

import random

class Agent:
    def __init__(self, id):
        # TODO: Make trainer, model and other attributes readable from a config file

        self.__player_id = id
        self.__trainer = DoubleDuelDQNTrainer()
        self.__sess = None
        self.__model = None
        self.__input_provider = NormalizedOneHotInputProvider()
        self.__is_training = True

    def action_step(self, game_state, screen_shot):
        if self.__is_training:
            game_state, screen_shot = self.__input_provider.store_and_retrieve(game_state, screen_shot)
            action = self.__trainer.train_step(game_state, screen_shot)
            return action
        else:
            game_state, screen_shot = self.__input_provider.store_and_retrieve(game_state, screen_shot)
            action = self.__model.prediction(game_state, screen_shot)
            return action

    # def action_step(self, game_state, screen_shot):
    #     #print('Agent taking action')
    #     action = random.randint(0, 22)
    #     print(game_state.player1.player_id, " , ", game_state.player2.player_id)
    #     print(game_state.player1.player_id, " , ", game_state.player2.player_id)
    #     return action
