# Generates CSharp properties for the .csv file
# in the form of .txt
import sys
import os

def get_columnsname(f):
    line = f.readline()
    columns = line.replace('\"', '').replace(' ', '').split(',')
    return columns;

def get_columnstype(f):
    line = f.readline()
    columns = line.replace('\"', '').split(',')
    for i in range(len(columns)):
        columns[i] = columns[i].lower()
        if(columns[i] == 'boolean'):
            columns[i] = 'bool'
    return columns

def gen_properties(f, n):
    outf = open(n, 'w')
    columnsname = get_columnsname(f)
    columnstype = get_columnstype(f)
    print('Generating {0} properties...'.format(len(columnsname)))
    for i in range(len(columnsname)):
        prop = 'public ' + columnstype[i] + ' ' + columnsname[i] + ' { get; set; }'
        outf.write(prop + '\r')
    outf.close()

def gen_allproperties(d):
    for filename in os.listdir(d):
        if filename.endswith(".csv"):
            f = open(os.path.join(d, filename))
            gen_properties(f, filename.replace('.csv', '_properties.txt'))

gen_allproperties(sys.argv[1])

