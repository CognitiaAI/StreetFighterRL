
class Buttons:

    def __init__(self, buttons_dict=None):

        self.__buttons = {}
        self.init_buttons()

    def init_buttons(self):
        self.up = False
        self.down = False
        self.right = False
        self.left = False
        self.select = False
        self.start = False
        self.Y = False
        self.B = False
        self.X = False
        self.A = False
        self.L = False
        self.R = False

    def dict_to_object(self, buttons_dict):

        self.up = buttons_dict['Up']
        self.down = buttons_dict['Down']
        self.right = buttons_dict['Right']
        self.left = buttons_dict['Left']
        self.select = buttons_dict['Select']
        self.start = buttons_dict['Start']
        self.Y = buttons_dict['Y']
        self.B = buttons_dict['B']
        self.X = buttons_dict['X']
        self.A = buttons_dict['A']
        self.L = buttons_dict['L']
        self.R = buttons_dict['R']

    def object_to_dict(self):

        buttons_dict = {}

        buttons_dict['Up'] = self.up
        buttons_dict['Down'] = self.down
        buttons_dict['Right'] = self.right
        buttons_dict['Left'] = self.left
        buttons_dict['Select'] = self.select
        buttons_dict['Start'] = self.start
        buttons_dict['Y'] = self.Y
        buttons_dict['B'] = self.B
        buttons_dict['X'] = self.X
        buttons_dict['A'] = self.A
        buttons_dict['L'] = self.L
        buttons_dict['R'] = self.R

        return buttons_dict

    def get_action_buttons(self):
        return [self.up, self.down, self.right, self.left, self.Y, self.B, self.X, self.A, self.L, self.R]