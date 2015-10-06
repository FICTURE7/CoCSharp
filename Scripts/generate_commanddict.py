# Generates C# code that adds ICommands in a specified directory to
# m_CommandDictionary in CommandFactory.cs

import os
import sys

def gen_addline(classname):
    return 'm_CommandDictionary.Add(new {0}().ID, typeof({0}));'.format(classname)

def gen_commanddict(directory):
    files = os.listdir(directory)
    dictstr = '// Generated from gen_commanddict.py\r\r'
    for f in files:
        if f.startswith('I'):
            continue # is interface

        print('Generating add line for {0}...'.format(f))
        classname = os.path.splitext(f)[0]
        dictstr += gen_addline(classname)

        if classname.endswith('Command') == False:
            dictstr += '// bad naming here!'
        dictstr += '\r'

    print('Done generating add lines!')
    return dictstr

def main():
    directory = sys.argv[1];
    directoryname = os.path.basename(directory)
    dictionaryfilename = '{0} dict.txt'.format(directoryname);

    print('Generating command dictionary for directory \r\t{0}...'.format(directory))
    dictstr = gen_commanddict(directory);

    print('Saving generated dictionary to {0}'.format(dictionaryfilename))
    dictfile = open(dictionaryfilename, 'w')
    dictfile.write(dictstr)
    dictfile.close()

if __name__ == '__main__':
    main()
