import TextModificationLib

# Entities the program should reject (Currently Hardcoded will be opened to configuration files later)
Replace_List = {'light_torch_small_walltorch': 'null', 'item_health': 'null', 'monster_zombie': 'null',
                'weapon_nailgun': 'null', 'item_spikes': 'null', 'info_player_deathmatch': 'null',
                'info_player_start2': 'info_player_start', 'func_episodegate': 'null'}
# Texture to replace and the values to replace them with
Texture_replacement_dictionary = {'TRIGGER': 'tools/toolstrigger', 'CLIP': 'tools/toolsclip'}
# base Texture Directory relative to the {game}/materials folder.
texture_dir = 'quake/'

# Entities the program will pack into the vmf. No reason to edit this aside from if you want a certain entity in all
# maps
final_entities = {}


def Parse_File():
    world_dictionary = {}
    print("Grabbing and proccessing entities..")
    file = Get_Contents_of_MAP_File()
    print("Creating entity dictionary...")
    entities = Create_Entity_Dictionary(file)
    print("Prepping worldspawn...")
    brushes = Prep_Brushes(entities[1])
    print("converting Worldspawn...")
    brushes = readSide(brushes, False)
    print("Storing Data...")
    world_dictionary["worldspawn"] = brushes
    del entities[1]
    world_dictionary["entities"] = entities
    print("Finished Parsing Data!")
    return world_dictionary


def Create_Entity_Dictionary(entities_list):
    raiseException = True
    entities_dict = {}
    i = 2
    try:
        for entity in entities_list:
            i += 1
            classname = Find_Attribute(entity, 'classname')
            if "worldspawn" in classname:
                entities_dict[1] = entity
                i -= 1
                raiseException = False
            else:
                entities_dict[i] = entity
                i += 1
        if raiseException:
            raise RuntimeError('worldspawn_not_found')
    except RuntimeError:
        print("ERROR: No worldspawn entity found, this is required!, exiting now...")
        exit(10)

    return entities_dict


def Prep_Brushes(world_spawn_item):
    brushes = TextModificationLib.Split_First_Instance(world_spawn_item, '(')
    brushes = TextModificationLib.split(brushes[1], '\n', True)
    sides = []
    for item in brushes:
        side = Parse_Brush(item)
        sides.append(side)
    return sides


def Get_Contents_of_MAP_File():
    entities = []
    append_string = ''
    buffer = []
    filename_internal = filename + ".map"
    MaxLines = 0
    Map_file = ''
    i = 0
    try:
        MaxLines = TextModificationLib.Count_Lines_Fast(filename_internal)
        Map_file = open(filename_internal, mode='r')
    except FileNotFoundError:
        print('ERROR: File has not been located!, please ensure the filename is in this location and that the path is'
              ' included if outside this directory')
        exit(7)
    for x in range(0, MaxLines):
        line = Map_file.readline()
        line_ascii = list(bytes(line, "ascii"))
        # Rotate list or if not big enough to qualify for a rotation just add another letter
        for each in line_ascii:
            if len(buffer) == 2:
                buffer = TextModificationLib.RotateList(buffer, -1)
                buffer[1] = each
            else:
                buffer.append(each)

            if buffer == [123, 10]:
                i += 1
                if i == 1:
                    append_string = ''
            elif buffer == [125, 10]:
                i -= 1
                if i == 0:
                    entities.append('{' + append_string)
            append_string = append_string + chr(each)

    return entities


def Create_BrushEntity_Brush(brush_data):
    string_brush = ''
    firstwrite = True
    for side in brush_data:
        if brush_data[side].get('ID') == 1:
            ending = EndBrushes(False, firstwrite)
            if firstwrite:
                firstwrite = False
            string_brush = string_brush + ending
        side_value = Make_Side_of_brush(brush_data[side])
        string_brush = string_brush + side_value
    return string_brush


def Parse_Brush(brush):
    # Removes comments
    BrushData = TextModificationLib.RemoveCPPStyleComments(brush)
    BrushData = TextModificationLib.recombine(BrushData)
    BrushData = TextModificationLib.Unwrap(BrushData, '{', '}', True)
    return BrushData


# Adds the basic starting info for Hammer this also makes it compatible with HL:A importing
def Write_VMF_File(world_data):
    firstwrite = True
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
            ending = EndBrushes(False, firstwrite)
            if firstwrite:
                firstwrite = False
            file_1.write(ending)
        side_value = Make_Side_of_brush(brush_data[side])
        file_1.write(side_value)
    print("finishing up brush data...")
    ending = EndBrushes(True)
    file_1.write(ending)
    print("Creating entity list...")
    Create_Entity_List(entities)
    print("Writing entity list...")
    for key in final_entities:
        file_1.write(final_entities[key])
    print(f"Written {filename}.vmf")


def Write_Attribute_Normal(name, value):
    return f'\t"{name}" "{value}"\n'


def Find_Attribute(ent_data, target_attribute, end_location="\n"):
    pass
    attribute = TextModificationLib.FindBetweenTwoValues(ent_data, f'"{target_attribute}"', f"{end_location}")
    if attribute is not None:
        attribute = TextModificationLib.remove(attribute, '"')
    else:
        attribute = ''
    return attribute


def Parse_Brush_Entity(entity_data):
    ent_brush_data = TextModificationLib.split(entity_data, '{')
    ent_brush_data[0] = '{' + ent_brush_data[0] + '}\n'
    ent_brush_data[1] = '{' + ent_brush_data[1] + '\n'
    for cut in ent_brush_data:
        if cut != ent_brush_data[0]:
            ent_brush_data[1] = ent_brush_data[1] + cut
    ent_brush_data[1] = TextModificationLib.replace(ent_brush_data[1], '}\n}', '}')
    return ent_brush_data


def Detect_Brush_Entity(entity_data):
    brush_entity = False
    if '}\n}' in entity_data:
        brush_entity = True
    return brush_entity


def Detect_Attributes(ent_data):
    attributes_dict = {}
    i = 0
    attributes_list = TextModificationLib.split(ent_data, '"\n')
    for each in attributes_list:
        attributes_list[i] = each + '"'
        attribute_name = TextModificationLib.FindBetweenTwoValues(each, '"', '"')
        attribute = Find_Attribute(each, TextModificationLib.FindBetweenTwoValues(each, '"', '"'))
        attributes_dict[attribute_name] = attribute.strip()
        i += 1
    return attributes_dict


def createEntity(ent_data, ent_id):
    Is_brush_entity = Detect_Brush_Entity(ent_data)
    attributes_dict = Detect_Attributes(ent_data)
    attributes_string = ''
    brush = ''

    if Is_brush_entity:
        data = Parse_Brush_Entity(ent_data)
        brush_data = data[1]
        brush_data = Prep_Brushes(brush_data)
        brush_data = readSide(brush_data)
        brush = Create_BrushEntity_Brush(brush_data)
        brush += "\n\t}"

    if attributes_dict['classname'] not in Replace_List:
        if not attributes_dict.get('classname', 'null') == 'null':
            for attribute in attributes_dict:
                attributes_string = attributes_string + Write_Attribute_Normal(attribute, attributes_dict[attribute])
            entity = f'\nentity\n{{\n\t"id" "{ent_id}"\n{attributes_string}\n{brush}}}\n'
            final_entities[ent_id] = entity
    elif Replace_List[attributes_dict['classname']] != 'null':
        classname = Replace_List[attributes_dict['classname']]
        for attribute in attributes_dict:
            if attribute != 'classname':
                attributes_string = attributes_string + Write_Attribute_Normal(attribute, attributes_dict[attribute])
            else:
                attributes_string = attributes_string + Write_Attribute_Normal('classname', classname)
        entity = f'\nentity\n{{\n\t"id" "{ent_id}"\n{attributes_string}\n{brush}}}\n'
        final_entities[ent_id] = entity
    else:
        pass


def Make_Side_of_brush(properties):
    global ID
    global texture_dir
    side: str = str(properties['points'])
    side = TextModificationLib.remove(side, ttr_multiple=["[", "]", ",", "'"])
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


def readSide(lines, mute=True):
    if not mute:
        print("Extracting Data")
    plane_dict = {}
    internal_sideID = 0
    counter = 0
    for line in lines:
        if "(" in line and ")" in line:
            internal_sideID += 1
            value = TextModificationLib.remove(line, "[")
            value = TextModificationLib.remove(value, "]")
            value = TextModificationLib.remove(value, "( ")
            value = TextModificationLib.replace(value, "  ", ' ')
            value = TextModificationLib.split(value, " ) ", True)
            Other_values = TextModificationLib.split(value[3], ' ', True)
            for item in Other_values:
                if item == ' ':
                    Other_values.remove(' ')
            if not len(Other_values) < 7:
                plane_dict[counter] = {"points": [
                    TextModificationLib.add_to_both_sides(value[0], "(", ") "),
                    TextModificationLib.add_to_both_sides(value[1], "(", ") "),
                    TextModificationLib.add_to_both_sides(value[2], "(", ") ")
                ],
                    "Material": Other_values[0],
                    'x_off': Other_values[1],
                    'y_off': Other_values[2],
                    'rot_angle': Other_values[3],
                    'x_scale': Other_values[4],
                    'y_scale': Other_values[5],
                    'ID': internal_sideID
                }
                counter += 1
            else:
                pass
        else:
            internal_sideID = 0

    return plane_dict


def Create_Entity_List(entities):
    i = 1
    for entity in entities:
        createEntity(entities[entity], i)
        i += 1


def get_all_sides(faces):
    map_data = readSide(faces)
    return map_data


def EndBrushes(end_file=False, first_write=False):
    if end_file:
        return '}}'
    if not first_write:
        return """\n\t\t}\n\tsolid\n\t{"""
    else:
        return "\n\tsolid\n\t{"


filename = input(':> ')
world = Parse_File()
Write_VMF_File(world)
