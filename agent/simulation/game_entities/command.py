from game_entities.buttons import Buttons


class Command:

    def __init__(self):

        self.player_buttons = Buttons()
        self.player2_buttons = Buttons()
        self.__command_dict = {}
        self.__type = "buttons"
        self.__player_count = 2
        self.__save_directory = "E:\Research\Reinforcement Learning\Street Fighter RL\emulator\Bizhawk\BizHawk-master\output\SNES\State\Street Fighter II Turbo (USA).Snes9x.QuickSave"
        self.__save_game_path = ""
        self.init_command_dict()

    def init_command_dict(self):
        self.__command_dict['p1'] = self.player_buttons.object_to_dict()
        self.__command_dict['p2'] = self.player2_buttons.object_to_dict()
        self.__command_dict['type'] = self.__type
        self.__command_dict['player_count'] = self.__player_count
        self.__command_dict['savegamepath'] = self.__save_game_path

    def set_actions(self, button_list, player_id='p1'):

        for button in button_list:
            self.__command_dict['p1'][button] = True
        return self.__command_dict

    def set_type(self, command_type):
        self.__command_dict['type'] = command_type

    def set_save_game_path(self, save_game_path):
        self.__save_game_path = self.__save_directory + save_game_path
        self.__command_dict['savegamepath'] = self.__save_game_path

    def reset(self):
        self.init_command_dict()

    @property
    def command_dict(self):
        return self.__command_dict