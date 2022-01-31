import numpy as np

from pipeline.input_provider.base_input_provider import BaseInputProvider


class NormalizedOneHotInputProvider(BaseInputProvider):
    def __init__(self):
        BaseInputProvider.__init__(self)
        self.__game_state = None
        self.__screen_shot = None

    def store(self, game_state, screen_shot):
        self.__game_state = game_state
        self.__screen_shot = screen_shot

    def min_max_scaling(self, X, X_min, X_max, range_min = 0, range_max = 1):
        X_std = (X - X_min) / (X_max - X_min)
        X_scaled = X_std * (range_max - range_min) + range_min
        return X_scaled

    def one_hot(self, X, length):
        encoded = np.zeros(length)
        encoded[X] = 1
        return encoded

    def pre_processing(self):
        # Normalizes the input and converts to numpy
        processed_state = self.normalize_state()
        processed_screen_shot = self.__screen_shot / 255.0

        return processed_state, processed_screen_shot

    def normalize_state(self):
        game_state = list()
        game_state.extend(self.one_hot(self.__game_state.player1.player_id, 12))
        game_state.append(self.min_max_scaling(self.__game_state.player1.health, 0, 176.0))  # Min Max Scaling
        game_state.append(self.min_max_scaling(self.__game_state.player1.x_coord, 0, 500.0))
        game_state.append(self.min_max_scaling(self.__game_state.player1.y_coord, 0, 192.0))
        game_state.extend(self.one_hot(self.__game_state.player1.is_jumping, 2))
        game_state.extend(self.one_hot(self.__game_state.player1.is_crouching, 2))
        game_state.extend(self.one_hot(self.__game_state.player1.is_player_in_move, 2))
        # game_state.append(self.__game_state.player1.move_id)
        game_state.extend(self.__game_state.player1.get_action_buttons())  # adding 10 more values

        game_state.extend(self.one_hot(self.__game_state.player2.player_id, 12))
        game_state.append(self.min_max_scaling(self.__game_state.player2.health, 0, 176.0))  # Min Max Scaling
        game_state.append(self.min_max_scaling(self.__game_state.player2.x_coord, 0, 500.0))
        game_state.append(self.min_max_scaling(self.__game_state.player2.y_coord, 0, 192.0))
        game_state.extend(self.one_hot(self.__game_state.player2.is_jumping, 2))
        game_state.extend(self.one_hot(self.__game_state.player2.is_crouching, 2))
        game_state.extend(self.one_hot(self.__game_state.player2.is_player_in_move, 2))
        # game_state.append(self.__game_state.player2.move_id)
        game_state.extend(self.__game_state.player2.get_action_buttons()) # adding 10 more values

        return np.array(game_state)

    def retrieve(self):
        return self.pre_processing()

    def store_and_retrieve(self, game_state, screen_shot):
        self.store(game_state, screen_shot)
        return self.retrieve()



