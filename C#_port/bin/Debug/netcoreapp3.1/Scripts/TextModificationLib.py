

# Splits a string int a list given a specified marker
def split(input_string, split_by, quick=True):
    text_split = ['']
    if not quick:
        split_instance = 0
        for item in input_string:
            if item == split_by:
                if text_split[split_instance] != '':
                    text_split.append('')
                    split_instance += 1
            else:
                text_split[split_instance] = text_split[split_instance] + item
    else:
        text_split = input_string.split(split_by)
    return text_split


# TTR (Text to replace)
def replace(input_string, ttr, replace_with):
    return input_string.replace(ttr, replace_with)


# TTR (Text to remove)
def remove(input_string, ttr='', ttr_multiple=None):
    step_1 = input_string
    if ttr == '':
        for text_to_remove in ttr_multiple:
            step_1 = replace(input_string, text_to_remove, "")
    else:
        step_1 = replace(input_string, ttr, "")
    return step_1


# Recombines a list file into a string
def recombine(list_text: list) -> str:
    list_to_str = ''.join([str(elem) for elem in list_text])
    return list_to_str


def Unwrap(input_string, left_side, right_side, add_back_newline=False):
    input_string = input_string.strip(left_side)
    input_string = input_string.strip(right_side + ' \n')
    if add_back_newline:
        input_string = input_string + ' \n' + right_side
    return input_string


# Adds something to both sides, can be used with "" on the side you do not want anything in to add something to the
# beginning or end. IE: add_to_both_sides('gg', '', 'f') will net 'ggf'
def add_to_both_sides(input_string, left_side, right_side):
    input_string = left_side + input_string + right_side
    return input_string


# Turns a delimited string into a variable used mostly for configuration library stuff
def String_To_Dictionary(string, line_split=";", value_split="="):
    dict_new = {}
    values = split(string, line_split, True)
    for item in values:
        key_value_pair = split(item, value_split, True)
        dict_new[key_value_pair[0]] = key_value_pair[1]
    return dict_new


def Read_file(file_to_read):
    try:
        # Opens file
        file = open(file_to_read, mode='r')
        # Reads file and saves it in Variable
        contents = file.read()
        # Close the file
        file.close()
        # Passes on the contents and makes sure the Global variable also has it
        return contents
    except FileNotFoundError:
        print('Error: File not Found')
        return None


# Checks the file for contents
def Check_File_Has_Contents(input_file):
    file = open(input_file, "r")
    # Sets the cursor essentially to 0 on the text file
    file.seek(0)
    # Check for any Text in the file
    data = file.read(100)
    # If there are no Characters then assume the file needs to be regenerated
    if len(data) == 0:
        setup = False
    # if not Just Boot normally.
    else:
        print("Mounting SpreadSheet!")
        setup = True
    return setup


# Use function to make string lower case and do case-less matching to ensure it matches and makes things case
# insensitive. Equivalent to .casefold()
def make_lowercase(string):
    string_rebuild = ''
    ascii_interp = list(bytes(string, "ascii"))
    for each in ascii_interp:
        each = int(each)
        if 90 > each > 65:
            each = each + 32
        string_rebuild = string_rebuild + chr(each)
    return string_rebuild


# I found this online and do not fully understand the specifics, The code reads the raw data which is many times
# Faster for getting the line numbers in large files
def Count_Lines_Fast(file_name):
    # Opens the file raw
    f = open(file_name, 'rb')
    # sets lines equal to 0
    lines = 0
    # sets the buffer size
    buf_size = 1024 * 1024
    # Reads the file raw this is quick for large files
    read_f = f.read
    # takes the contents of read_f and gets buffer size
    buf = read_f(buf_size)
    # Past this point I can only guess as to it just counting the times new line is used and using that to get it's
    # line count This was the only reasonable way I could get the amount of lines
    while buf:
        # Finds out hw many \n things there are?
        lines += buf.count(b'\n')
        # Reads buffer size
        buf = read_f(buf_size)
    # Return Lines
    return lines


def RemoveCPPStyleComments(string_input):
    lines = split(string_input, '\n', True)
    lines_dict = {}
    number = 0
    for line in lines:
        lines_dict[number] = line
        number += 1
    for x in range(1, 2):
        for line in lines_dict:
            if '//' in lines_dict[line]:
                lines_dict[line] = remove(lines_dict[line], FindBetweenTwoValues(lines_dict[line], '//', '\n'))
                lines_dict[line] = remove(lines_dict[line], '//')
    lines.clear()
    for each in lines_dict:
        if lines_dict[each] is not None:
            lines.append(lines_dict[each] + '\n')
    return lines


def FindBetweenTwoValues(Search, firstIndex, LastIndex, suppress_error=True):
    try:
        return (Search.split(firstIndex))[1].split(LastIndex)[0]
    except IndexError:
        if not suppress_error:
            print(Search, firstIndex, LastIndex)
