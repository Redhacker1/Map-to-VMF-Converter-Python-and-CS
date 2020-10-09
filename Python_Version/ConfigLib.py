import os
from tkinter import filedialog

import Python_Version.TextModificationLib as TextModificationLib
import Python_Version.List_Dict_Array_Library as List_Dict_Array_Library

# Entities the program should replace (Currently Hardcoded, will be opened to configuration files later.)
Replace_List = {'light_torch_small_walltorch': 'null', 'item_health': 'null', 'monster_zombie': 'null',
                'weapon_nailgun': 'null', 'item_spikes': 'null', 'info_player_deathmatch': 'null',
                'info_player_start2': 'null', 'func_episodegate': 'null'}

# Texture to replace and the values to replace them with, again will be opened to config files later.
Texture_replacement_dictionary = {'TRIGGER': 'tools/toolstrigger', 'CLIP': 'tools/toolsclip'}

# List of Dictionaries containing entities, which contain a dictionary of Replacement keys,
# and a list and how the values should be treated
Entity_Param_Replacement_Dict = {}

# base Texture Directory relative to the {game}/materials folder, default is 'quake/'. It will be changeable in the
# config file as well
texture_dir = 'quake/'

# Entities the program will pack into the vmf, No reason to edit this aside from if you want a certain entity in all
# maps, entites if such a method is used should adhere to the internal structure used by the script
final_entities = {}


def parse_file():
    world_dictionary = {}
    print("Grabbing and processing entities..")
    file = get_contents_of_map_file()
    print("Creating entity dictionary...")
    entities = create_entity_dictionary(file)
    print("Preparing worldspawn...")
    brushes = prep_brushes(entities[1])
    print("Converting Worldspawn...")
    brushes = read_side(brushes, False)
    print("Storing data...")
    world_dictionary["worldspawn"] = brushes
    del entities[1]
    world_dictionary["entities"] = entities
    print("Finished parsing data!")
    return world_dictionary


def create_entity_dictionary(entities_list):
    raise_exception = True
    entities_dict = {}
    i = 2
    try:
        for entity in entities_list:
            i += 1
            classname = find_attribute(entity, 'classname')
            if "worldspawn" in classname:
                entities_dict[1] = entity
                i -= 1
                raise_exception = False
            else:
                entities_dict[i] = entity
                i += 1
        if raise_exception:
            raise RuntimeError('worldspawn not found')
    except RuntimeError:
        print("ERROR: No worldspawn entity found, this is required!, exiting now...")
        exit(-1)

    return entities_dict


def prep_brushes(world_spawn_item):
    brushes = List_Dict_Array_Library.split_first_instance(world_spawn_item, '(')
    brushes = TextModificationLib.split(brushes[1], '\n')
    sides = []
    for item in brushes:
        side = parse_brush(item)
        sides.append(side)
    return sides


def get_contents_of_map_file():
    entities = []
    append_string = ''
    filename_internal = filename + ".map"
    max_lines = 0
    map_file = ''
    i = 0
    try:
        max_lines = TextModificationLib.count_lines_fast(filename_internal)
        map_file = open(filename_internal)
    except FileNotFoundError:
        print('ERROR: File has not been located!, please ensure the filename is in this location and that the path is'
              ' included if outside this directory')
        exit(-2)
    for _ in range(0, max_lines):
        line = map_file.readline()
        line_ascii = list(bytes(line, "ascii"))
        previous_char = ""
        for letter in line_ascii:
            if letter == 10:
                if previous_char == 123:
                    i += 1
                    if i == 1:
                        append_string = ''
                elif previous_char == 125:
                    i -= 1
                    if i == 0:
                        entities.append('{' + append_string)
            append_string = append_string + chr(letter)
            previous_char = letter
    return entities


def create_brush_entity_brush(brush_data):
    string_brush = ''
    first_write = True
    for side in brush_data:
        if brush_data[side].get('ID') == 1:
            ending = end_brushes(first_write=first_write)
            if first_write:
                first_write = False
            string_brush = string_brush + ending
        side_value = make_side_of_brush(brush_data[side])
        string_brush = string_brush + side_value
    return string_brush


def parse_brush(brush):
    # Removes comments
    brush_data = TextModificationLib.remove_cpp_style_comments(brush)
    brush_data = TextModificationLib.recombine(brush_data)
    brush_data = TextModificationLib.unwrap(brush_data, '{', '}', True)
    return brush_data


# Adds the basic starting info for Hammer this, the versioninfo and viewsettings bits are not important for loading with
# source 1 hammer but makes it compatible with source 2 hammer.

def write_vmf_file(world_data):
    first_write = True
    print("Writing Data, opening destination file...")
    file_1 = open(filename + '.vmf', 'w')
    file_1.write('versioninfo\n'
                 '{\n'
                 '}\n'
                 '\n'
                 'viewsettings\n'
                 '{\n'
                 '}\n'
                 '\n'
                 'world\n'
                 '{\n'
                 f'	"id" "{1}"\n'
                 '	"mapversion" "1"\n'
                 '	"classname" "worldspawn"\n'
                 )
    print("retrieving brush data...")
    brush_data = world_data["worldspawn"]
    print("retrieving entity data...")
    entities = world_data['entities']
    print("Writing Brush data...")
    for side in brush_data:
        if brush_data[side].get('ID') == 1:
            ending = end_brushes(first_write=first_write)
            if first_write:
                first_write = False
            file_1.write(ending)
        side_value = make_side_of_brush(brush_data[side])
        file_1.write(side_value)
    print("finishing up brush data...")
    ending = end_brushes(True)
    file_1.write(ending)
    print("Creating entity list...")
    create_entity_list(entities)
    print("Writing entity list...")
    for key in final_entities:
        file_1.write(final_entities[key])
    print(f"Written {filename}.vmf")


def write_attribute_normal(name, value):
    return f'\t"{name}" "{value}"\n'


def find_attribute(ent_data, target_attribute, end_location="\n"):
    attribute = TextModificationLib.find_between_two_values(ent_data, f'"{target_attribute}"', f"{end_location}")
    if attribute is not None:
        attribute = TextModificationLib.remove(attribute, '"')
    else:
        attribute = ''
    return attribute


def parse_brush_entity(entity_data):
    ent_brush_data = TextModificationLib.split(entity_data, '{')
    ent_brush_data[0] = '{' + ent_brush_data[0] + '}\n'
    ent_brush_data[1] = '{' + ent_brush_data[1] + '\n'
    for cut in ent_brush_data:
        if cut != ent_brush_data[0]:
            ent_brush_data[1] = ent_brush_data[1] + cut
    ent_brush_data[1] = TextModificationLib.replace(ent_brush_data[1], '}\n}', '}')
    return ent_brush_data


def detect_brush_entity(entity_data):
    brush_entity = False
    if '}\n}' in entity_data:
        brush_entity = True
    return brush_entity


def detect_attributes(ent_data):
    attributes_dict = {}
    i = 0
    attributes_list = TextModificationLib.split(ent_data, '"\n')
    for each in attributes_list:
        attributes_list[i] = each + '"'
        attribute_name = TextModificationLib.find_between_two_values(each, '"', '"')
        attribute = find_attribute(each, TextModificationLib.find_between_two_values(each, '"', '"'))
        attributes_dict[attribute_name] = attribute.strip()
        i += 1
    return attributes_dict


def create_entity(ent_data, ent_id):
    is_brush_entity = detect_brush_entity(ent_data)
    attributes_dict = detect_attributes(ent_data)
    attributes_string = ''
    brush = ''

    if is_brush_entity:
        data = parse_brush_entity(ent_data)
        brush_data = data[1]
        brush_data = prep_brushes(brush_data)
        brush_data = read_side(brush_data)
        brush = create_brush_entity_brush(brush_data)
        brush += "\n\t}"

    if attributes_dict['classname'] not in Replace_List:
        if not attributes_dict.get('classname', 'null') == 'null':
            for attribute in attributes_dict:
                attributes_string = attributes_string + write_attribute_normal(attribute, attributes_dict[attribute])
            entity = f'\nentity\n{{\n\t"id" "{ent_id}"\n{attributes_string}\n{brush}}}\n'
            final_entities[ent_id] = entity
    elif Replace_List[attributes_dict['classname']] != 'null':
        classname = Replace_List[attributes_dict['classname']]
        for attribute in attributes_dict:
            if attribute != 'classname':
                attributes_string = attributes_string + write_attribute_normal(attribute, attributes_dict[attribute])
            else:
                attributes_string = attributes_string + write_attribute_normal('classname', classname)
        entity = f'\nentity\n{{\n\t"id" "{ent_id}"\n{attributes_string}\n{brush}}}\n'
        final_entities[ent_id] = entity
    else:
        pass


def make_side_of_brush(properties):
    global texture_dir
    side: str = str(properties['points'])
    side = side.strip()
    side = TextModificationLib.replace(side, ") ", ")")
    side = TextModificationLib.remove(side, "[")
    side = TextModificationLib.remove(side, "]")
    side = TextModificationLib.remove(side, ",")
    if properties['Material'] not in Texture_replacement_dictionary:
        texture = texture_dir + TextModificationLib.remove(properties['Material'], ttr_multiple=['#', '*'])
        texture = texture.casefold()
    else:
        texture = Texture_replacement_dictionary[properties['Material']]
    y_scale = properties['y_scale']
    x_scale = properties['x_scale']
    offset_x = properties['x_off']
    offset_y = properties['y_off']
    plane = f"""
        side
        {{
            "id" "{properties['ID']}"
            "plane" "{side}"
            "material" "{texture}"
            "uaxis" "[0 0 0 {offset_x}] {x_scale}"
            "vaxis" "[0 0 0 {offset_y}] {y_scale}"
            "rotation" "{properties['rot_angle']}"
            "lightmapscale" "16"
            "smoothing_groups" "0"
        }}"""
    return plane


def read_side(lines, mute=True):
    if not mute:
        print("Extracting Data")
    plane_dict = {}
    internal_side_id = 0
    counter = 0
    for line in lines:
        if "(" in line and ")" in line:
            internal_side_id += 1
            value = TextModificationLib.remove(line, "[")
            value = TextModificationLib.remove(value, "]")
            value = TextModificationLib.remove(value, "( ")
            value = TextModificationLib.replace(value, "  ", ' ')
            value = TextModificationLib.split(value, " ) ")
            other_values = TextModificationLib.split(value[3], ' ')
            for item in other_values:
                if item == ' ':
                    other_values.remove(' ')
            if not len(other_values) < 7:
                plane_dict[counter] = {"points": [
                    TextModificationLib.add_to_both_sides(value[0], "(", ") "),
                    TextModificationLib.add_to_both_sides(value[1], "(", ") "),
                    TextModificationLib.add_to_both_sides(value[2], "(", ") ")
                ],
                    "Material": other_values[0],
                    'x_off': other_values[1],
                    'y_off': other_values[2],
                    'rot_angle': other_values[3],
                    'x_scale': other_values[4],
                    'y_scale': other_values[5],
                    'ID': internal_side_id
                }
                counter += 1
            else:
                pass
        else:
            internal_side_id = 0
    print(plane_dict[1])

    return plane_dict


def create_entity_list(entities):
    i = 1
    for entity in entities:
        create_entity(entities[entity], i)
        i += 1


def end_brushes(end_file=False, first_write=False):
    if end_file:
        return '}}'
    if not first_write:
        return """\n\t\t}\n\tsolid\n\t{"""
    else:
        return "\n\tsolid\n\t{"


def browse_map_file():
    filename_internal = filedialog.askopenfilename(initialdir=os.getcwd(), title="Select a File",
                                                   filetypes=(("Quake 1 non-compiled map file (.map) ", "*.map"),
                                                              ("all files", "*.*")))
    return filename_internal


filename = browse_map_file()
filename = TextModificationLib.remove(TextModificationLib.make_lowercase(filename), ".map")
print(filename)
world = parse_file()
write_vmf_file(world)
