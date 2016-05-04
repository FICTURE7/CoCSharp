# Converts a hex string into a valid C# byte array.

import argparse

def main():
    parser = argparse.ArgumentParser(description='Converts a hex-string into a valid C# byte array.')
    parser.add_argument('hex',
                        help='hex-string to convert')
    parser.add_argument('-n', '--name',
                        help='variable name of the C# byte array')

    args = parser.parse_args()
    name = args.name
    hexstr = args.hex

    if name == None:
        name = 'array'

    final = 'var {0} = new byte[] {{ '.format(name)

    for i in range(0, len(hexstr), 2):
        value = hexstr[i:i + 2]
        final += '0x{0}'.format(value.upper())

        if i != len(hexstr) - 2:
            final += ', '

    final += ' };'

    print(final)

if __name__ == "__main__":
    main();
