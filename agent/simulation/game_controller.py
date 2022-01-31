from game_entities.agent import Agent
from util.action_mapper import ActionMapper
from game_entities.command import Command


class GameController:
    def __init__(self, id, mode):
        self.__agent = Agent(id)
        self.__super_move = []
        self.__command = Command()
        self.action_mapper = ActionMapper()
        self.__mode = mode
        self.__current_save_state = 1
        self.__rounds = 0
        self.__current_state = None
        self.__new_state = None
        self.__skip_start = 30

    def step(self, game_state, screen_shot):

        self.__command = Command()
        if self.__skip_start > 0:
            self.__skip_start -= 1
            return self.__command
        if game_state.player1.is_player_in_move == 1:
            return self.__command
        elif game_state.fight_result == 'NOT_OVER' and len(self.__super_move) == 0:
            maneuver = self.__agent.action_step(game_state, screen_shot)
            action_list = self.action_mapper.get_action_list(maneuver, game_state) # Game State need by mapper to mirror super move if needed

            if 19 < maneuver < 23:
                self.__super_move = action_list
            else:
                self.__command.set_actions(action_list)
        elif len(self.__super_move) == 0:
            self.__command.reset()
            self.__command.set_type('reset')

            if self.__mode == 'testing':
                current_state = 11
            self.__command.set_save_game_path(str(self.__current_save_state) + '.State')

            self.__rounds += 1
            if self.__rounds % 5 == 0:
                self.__current_save_state = (self.__current_save_state + 1) % 10
                self.__rounds = 0

        if len(self.__super_move) > 0:
            self.__command.reset()
            self.__command.set_actions(self.__super_move[0])
            self.__super_move = self.__super_move[1:]

        return self.__command
