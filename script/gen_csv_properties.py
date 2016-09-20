# Generates C# documented properties for the .csv file
# in the form of .txt
import sys
import os

# Format the specified string into a string that is
# close to what the C# documentation needs.
def format_documentation_name(name):
    formatted = ''
    for i in range(len(name)):
        # Check if its uppercased but not first character.
        if i != 0 and name[i].isupper():
            # Add a space between the character.
            formatted += ' ' + name[i].lower()
            continue

        # Replace '_' characters with a space ' '.
        if name[i] == '_':
            formatted += ' ';
            continue

        formatted += name[i]

    return formatted

# Gets the columns names from the specified file.
def get_columnsname(f):

    # Read a single line then trim the last character (\n).
    line = f.readline()
    line = line[:len(line) - 1]

    columns = line.replace('\"', '').replace(' ', '').split(',')
    return columns;

# Gets the columns types from the specified file.
# Must also make sure that the readline() returns the second line.
def get_columnstype(f):

    # Read a single line then trim the last character (\n).
    line = f.readline()
    line = line[:len(line) - 1]

    columns = line.replace('\"', '').split(',')
    for i in range(len(columns)):
        columns[i] = columns[i].lower()

        # Change type string 'boolean' to 'bool' for the C# type.
        if(columns[i] == 'boolean'):
            columns[i] = 'bool'

    return columns

# Generate the properties for the specified file and
# outputs its at the specified file path.
def gen_properties(f, n):
    # Output file.
    outf = open(n, 'w')

    columnsname = get_columnsname(f)
    columnstype = get_columnstype(f)

    print('Generating {0} properties...'.format(len(columnsname)))

    # Get filename from path.
    filename = os.path.split(f.name)[1]

    # Add some comment about from where it was generated.
    outf.write('// NOTE: This was generated from the ' + filename + ' using gen_csv_properties.py script.\n\n')

    # Iterate through columnsname and genrate its property.
    for i in range(len(columnsname)):
        # Add the documentation to the property.
        prop = '/// <summary>\n/// Gets or sets ' + format_documentation_name(columnsname[i]) + '.\n/// </summary>\n'
        # Add the property type and property name.
        prop += 'public ' + columnstype[i] + ' ' + columnsname[i] + ' { get; set; }'
        outf.write(prop + '\n')

    outf.close()

def gen_allproperties(d):
    for filename in os.listdir(d):

        # Iterates through all the available files and check if
        # its extention is ".csv".
        if filename.endswith(".csv"):

            # Open the file and generate the properties.
            f = open(os.path.join(d, filename))
            gen_properties(f, filename.replace('.csv', '_properties.txt'))

def main():
    gen_allproperties(sys.argv[1])

if __name__ == "__main__":
    main()
