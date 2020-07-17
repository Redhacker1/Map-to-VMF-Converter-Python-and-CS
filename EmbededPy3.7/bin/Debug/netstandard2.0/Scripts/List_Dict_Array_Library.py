import collections
from Scripts.TextModificationLib import *


# Turns a delimited string into a variable used mostly for configuration library stuff
def String_To_Dictionary(string, line_split=";", value_split="="):
    dict_new = {}
    values = split(string, line_split, True)
    for item in values:
        key_value_pair = split(item, value_split, True)
        dict_new[key_value_pair[0]] = key_value_pair[1]
    return dict_new


def RotateList(list_to_rotate, rotate_by):
    rotate_list = collections.deque(list_to_rotate)
    rotate_list.rotate(rotate_by)
    rotate_list = list(rotate_list)
    return rotate_list


def python_list_to_Human_list(text):
    # Grabs the text one line_number at a time to format it
    text_intermediate = str(text)
    # Removes extra space in beginning keeping with excessive use the text flying off into space
    text_intermediate = replace(text_intermediate, ', ', ',')
    # Removes Square bracket Left
    text_intermediate = remove(text_intermediate, '[')
    # Removes Square Bracket right
    text_intermediate = remove(text_intermediate, ']')
    # Removes single quote in values
    text_intermediate = remove(text_intermediate, "'")
    # Prints for Debugging (currently disabled)
    # print(text_intermediate)
    # Writes it and a new line_number at the end
    return text_intermediate


def Deferred_Split(text, split_by, times_to_defer=2):
    text_split = ['']
    split_instance = 0
    for item in text:
        if item == split_by and split_instance == times_to_defer:
            print("match found")
            if text_split[split_instance] != '':
                text_split.append('')
                split_instance += 1
        elif split_instance <= times_to_defer:
            text_split[split_instance] = text_split[split_instance] + item
        else:
            break
    text_split[1] = remove(text, text_split[0])
    return text_split


def Split_First_Instance(text, split_by):
    text_split = ['']
    split_instance = 0
    for item in text:
        if item == split_by and split_instance == 0:
            if text_split[split_instance] != '':
                text_split.append('')
                split_instance += 1
        elif split_instance == 0:
            text_split[split_instance] = text_split[split_instance] + item
        else:
            break
    cheat_string = remove(text, text_split[0])
    text_split[1] = cheat_string
    return text_split
