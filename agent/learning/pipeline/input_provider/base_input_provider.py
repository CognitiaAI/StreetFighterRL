import numpy as np


class BaseInputProvider:
    def __init__(self):
        self.__game_state = None
        self.__screen_shot = None

    def store(self, game_state, screen_shot):
        self.__game_state = game_state
        self.__screen_shot = screen_shot

    def pre_processing(self):
        # Normalizes the input and converts to numpy
        processed_state = self.normalize_state()
        processed_screen_shot = self.__screen_shot / 255.0

        return processed_state, processed_screen_shot

    def normalize_state(self):
        game_state = list()
        game_state.append(self.__game_state.player1.player_id)
        game_state.append(self.__game_state.player1.health)
        game_state.append(self.__game_state.player1.x_coord)
        game_state.append(self.__game_state.player1.y_coord)
        game_state.append(self.__game_state.player1.is_jumping)
        game_state.append(self.__game_state.player1.is_crouching)
        game_state.append(self.__game_state.player1.is_player_in_move)
        # game_state.append(self.__game_state.player1.move_id)
        game_state.extend(self.__game_state.player1.get_action_buttons())  # adding 10 more values

        game_state.append(self.__game_state.player2.player_id)
        game_state.append(self.__game_state.player2.health)
        game_state.append(self.__game_state.player2.x_coord)
        game_state.append(self.__game_state.player2.y_coord)
        game_state.append(self.__game_state.player2.is_jumping)
        game_state.append(self.__game_state.player2.is_crouching)
        game_state.append(self.__game_state.player2.is_player_in_move)
        # game_state.append(self.__game_state.player2.move_id)
        game_state.extend(self.__game_state.player2.get_action_buttons()) # adding 10 more values

        return np.array(game_state)

    def retrieve(self):
        return self.pre_processing()

    def store_and_retrieve(self, game_state, screen_shot):
        self.store(game_state, screen_shot)
        return self.retrieve()



