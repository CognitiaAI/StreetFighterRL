import tensorflow as tf


class CNNDuelDQNModel:
    def __init__(self, name, sess):
        self.name = name
        self.__sess = sess
        self.n_inputs = 62
        self.n_hidden1 = 256
        self.n_hidden2 = 128
        self.n_hidden3 = 64
        self.n_outputs = 25
        self.optimizer = 'adam'
        self.initializer = tf.contrib.layers.xavier_initializer()

        with tf.variable_scope(name) as scope:
            self.state = tf.placeholder(tf.float32, shape=[None, self.n_inputs])
            self.image = tf.placeholder(tf.float32, shape=[None, 224, 256, 3])
            self.learning_rate = tf.placeholder_with_default(tf.constant(0.0001), shape=[])

            # Convolutional layers
            conv1 = tf.layers.conv2d(
                inputs=self.image, filters=32, kernel_size=[8, 8], strides=4,
                kernel_initializer=self.initializer,
                padding="valid", activation=tf.nn.relu, use_bias=False, name='conv1')
            conv2 = tf.layers.conv2d(
                inputs=conv1, filters=64, kernel_size=[4, 4], strides=2,
                kernel_initializer=self.initializer,
                padding="valid", activation=tf.nn.relu, use_bias=False, name='conv2')
            conv3 = tf.layers.conv2d(
                inputs=conv2, filters=64, kernel_size=[3, 3], strides=1,
                kernel_initializer=self.initializer,
                padding="valid", activation=tf.nn.relu, use_bias=False, name='conv3')
            conv4 = tf.layers.conv2d(
                inputs=conv3, filters=256, kernel_size=[7, 7], strides=1,
                kernel_initializer=self.initializer,
                padding="valid", activation=tf.nn.relu, use_bias=False, name='conv4')

            flat = tf.layers.flatten(conv4, name='flat')
            conv_dense1 = tf.layers.dense(flat, 128, kernel_initializer=self.initializer, name='conv_dense1')
            self.active_conv = tf.nn.relu(conv_dense1)

            hidden1 = tf.layers.dense(self.state, self.n_hidden1, kernel_initializer=self.initializer)
            # mean1, var1 = tf.nn.moments(hidden1, axes=[1])
            # norm1 = (hidden1 - mean1) / tf.sqrt(var1)
            self.active1 = tf.nn.relu(hidden1)

            hidden2 = tf.layers.dense(self.active1, self.n_hidden2, kernel_initializer=self.initializer)
            # mean2, var2 = tf.nn.moments(hidden2, axes=[1])
            # norm2 = (hidden2 - mean2)/tf.sqrt(var2)
            self.active2 = tf.nn.relu(hidden2)

            concat = tf.concat([self.active2, self.active_conv], axis=1, name="concat")

            hidden4 = tf.layers.dense(concat, 64, kernel_initializer=self.initializer)
            self.active4 = tf.nn.relu(hidden4)

            # Splitting into value and advantage stream
            value_stream, advantage_stream = tf.split(self.active4, 2, 1)
            value_stream = tf.layers.flatten(value_stream)
            advantage_stream = tf.layers.flatten(advantage_stream)
            advantage = tf.layers.dense(
                inputs=advantage_stream, units=self.n_outputs,
                kernel_initializer=self.initializer, name="advantage")
            value = tf.layers.dense(
                inputs=value_stream, units=1,
                kernel_initializer=self.initializer, name='value')

            # Combining value and advantage into Q-values as described above
            self.q_values = value + tf.subtract(advantage, tf.reduce_mean(advantage, axis=1, keepdims=True))
            self.predict = tf.argmax(self.q_values, 1)

        self.trainable_vars = tf.get_collection(tf.GraphKeys.TRAINABLE_VARIABLES, scope=scope.name)
        self.trainable_vars_by_name = {var.name[len(scope.name):]: var for var in self.trainable_vars}

        with tf.variable_scope(name+"/train"):
            self.action = tf.placeholder(tf.int32, shape=[None])
            self.y = tf.placeholder(tf.float32, shape=[None, self.n_outputs])
            # self.y = tf.placeholder(tf.float32, shape=[None, 1])
            # q_value = tf.reduce_sum(self.q_values * tf.one_hot(self.action, self.n_outputs),
            #                         axis=1, keep_dims=True)
            # error = tf.abs(self.y - q_value)
            # clipped_error = tf.clip_by_value(error, 0.0, 1.0)
            # linear_error = 2 * (error - clipped_error)
            # self.loss = tf.reduce_mean(tf.square(clipped_error) + linear_error)
            self.loss = tf.reduce_sum(tf.square(self.y - self.q_values))

            self.global_step = tf.Variable(0, trainable=False, name='global_step')
            optimizer = tf.train.AdamOptimizer(self.learning_rate)
            self.training_op = optimizer.minimize(self.loss, global_step=self.global_step)

        self.init = tf.global_variables_initializer()
        self.saver = tf.train.Saver()

    def prediction(self, game_state):
        action = self.__sess.run(self.predict, feed_dict={self.state: game_state})
        return action

    @property
    def sess(self):
        return self.__sess