from game_entities.buttons import Buttons

class Player:

    def __init__(self, player_dict):
        
        self.dict_to_object(player_dict)
    
    def dict_to_object(self, player_dict):
        
        self.player_id = player_dict['character']
        self.health = player_dict['health']
        self.x_coord = player_dict['x']
        self.y_coord = player_dict['y']
        self.is_jumping = player_dict['jumping']
        self.is_crouching = player_dict['crouching']
        self.player_buttons = Buttons(player_dict['buttons'])
        self.is_player_in_move = player_dict['in_move']
        # self.move_id = player_dict['move']

    def get_action_buttons(self):
        return self.player_buttons.get_action_buttons()