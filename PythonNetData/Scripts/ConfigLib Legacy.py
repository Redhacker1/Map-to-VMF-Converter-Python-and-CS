import TextModificationLib

legacy = False
entID = 1
apple = 0
# all entities in the map
entities_dict = {}
# Entities the program can take (Currently Hardcoded)
valid_ents = {" light", " info_player_start"}
brush_ents = {"trigger", "worldspawn"}
# Entities the program will pack into the vmf
final_entities = {}
# a list of brushes in the map
world_spawn = []


def ParseMapFile_new(filename_1):
    global world_spawn
    global legacy
    try:
        Map_Data = open(filename_1, mode='r')
        Map_Data = Map_Data.read()
        data = TextModificationLib.split(Map_Data, "//", False)
        i = 0
        b = 0
        for item in data:
            if "brush " in item:
                file_ID = int(TextModificationLib.FindBetweenTwoValues(item, " brush ", "\n"))
                if file_ID != b:
                    # print(file_ID)
                    item: str = TextModificationLib.remove(item, f"brush {b}")
                    world_spawn.append(item)
                    b += 1
            if "entity " in item:
                stop = False
                for each in brush_ents:
                    if each not in item and not stop:
                        i += 1
                        item = TextModificationLib.remove(item, f"entity {i}")
                        entities_dict[i] = item
                    else:
                        stop = True
            else:
                pass
        try:
            world_spawn[10]
        except KeyError and IndexError:
            print("Unsupported Map File Detected, Please save in Trenchbroom to get a compatible map file.\n"
                  "Parsing brush data anyway, NOTE: Entities will NOT be converted")
            world_spawn = Parse_Brush(Map_Data, Entire_map=True)
            legacy = True


    except FileNotFoundError:
        print("File not found, Exiting")
        exit()


def Parse_Brush(brush, Entire_map=False):
    try:
        Map_Data = brush
        # Removes comments
        if not Entire_map:
            brush_number = TextModificationLib.FindBetweenTwoValues(Map_Data, "brush", "\n")
            Map_Data = TextModificationLib.remove(Map_Data, "brush" + brush_number)
        data = TextModificationLib.RemoveCPPStyleComments(Map_Data)
        data = TextModificationLib.recombine(data)
        data = TextModificationLib.Unwrap(data, '{', '}', True)
        data = TextModificationLib.split(data, "\n", False)
        return data

    except FileNotFoundError:
        print("File Not found, Exiting")
        exit()


# Adds the basic starting info for Hammer this also makes it compatible with HL:A importing
def writeVMF(map_data):
    print("Writing Data")
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
                 f'	"id" "{entID}"\n'
                 '	"mapversion" "1"\n'
                 '	"classname" "worldspawn"\n'
                 '	solid'
                 '	{')
    plane_value = Make_Plane(map_data[1])
    file_1.write(plane_value)
    for side in map_data:
        if side != 1:
            if map_data[side].get('ID') == 1:
                ending = EndBrushes(False)
                file_1.write(ending)
            plane_value = Make_Plane(map_data[side])
            file_1.write(plane_value)
    ending = EndBrushes(True)
    file_1.write(ending)
    Create_Entity_List()
    for key in final_entities:
        file_1.write(final_entities[key])


def FindAttribute(ent_data, target_attribute, end_location="\n"):
    attribute = TextModificationLib.FindBetweenTwoValues(ent_data, f'"{target_attribute}"', f"{end_location}")
    attribute = TextModificationLib.remove(attribute, '"')
    return attribute




def createEntity(ent_data, ent_id):
    classname: str = FindAttribute(ent_data, "classname")
    classname = classname.strip()
    if classname in valid_ents:

        entity = ('entity\n'
                  '{\n'
                  f'	"id" "{ent_id}"\n'
                  f'	"classname" "{classname}"\n'
                  f'	"origin" "{FindAttribute(ent_data, "origin")}"\n'
                  f'	"angle" "{"0 0 0"}"\n'
                  '}\n\n')
        final_entities[ent_id] = entity
    else:
        pass


def Make_Plane(properties):
    global ID
    global ID
    side: str = str(properties['points'])
    side = TextModificationLib.remove(side, ttr_multiple=["[", "]", ",", "'"])
    side = side.strip()
    side = TextModificationLib.replace(side, ") ", ")")
    side = TextModificationLib.remove(side, "[")
    side = TextModificationLib.remove(side, "]")
    side = TextModificationLib.remove(side, ",")
    texture_dir = 'quake/'
    texture = texture_dir + TextModificationLib.remove(properties['Material'], ttr_multiple=['#', '*'])
    texture = texture.casefold()
    y_scale = properties['y_scale']
    x_scale = properties['x_scale']
    offset_x = int(properties['x_off'])
    offset_y = int(properties['y_off'])
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


def readSide(lines):
    print("Extracting Data")
    plane_dict = {}
    internal_sideID = 0
    counter = 0
    if not legacy:
        for item in lines:
            item = Parse_Brush(item)
            for thing in item:
                if "(" in thing and ")" in thing:
                    internal_sideID += 1
                    value = TextModificationLib.remove(thing, "( ")
                    value = TextModificationLib.split(value, " ) ", False)
                    Other_values = TextModificationLib.split(value[3], ' ', False)
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
                    internal_sideID = 0

    if legacy:
        for item in lines:
            if "(" in item and ")" in item:
                internal_sideID += 1
                value = TextModificationLib.remove(item, "( ")
                value = TextModificationLib.split(value, " ) ", False)
                Other_values = TextModificationLib.split(value[3], ' ', False)
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

    return plane_dict


def Create_Entity_List():
    i = 1
    #print(entities_dict)
    for entities in entities_dict:
        createEntity(entities_dict[entities], i)
        i += 1
    #print(final_entities)


def get_all_sides(faces):
    map_data = readSide(faces)
    return map_data


def EndBrushes(end_file=False, first_write=False):
    if end_file:
        return '}}'
    if not first_write:
        return """
                    }

           solid    {"""
    else:
        return "\n"


filename = input(':> ')
file = filename + '.map'
ParseMapFile_new(filename_1=file)
print(world_spawn)
geo = get_all_sides(world_spawn)
writeVMF(geo)
