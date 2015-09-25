import shutil

def main():
    print('Copying plugin...')
    shutil.copyfile('../CoCSharp.Client.ExamplePlugin/bin/Debug/CoCSharp.Client.ExamplePlugin.dll', '../CoCSharp.Client/bin/Debug/plugins/CoCSharp.Client.ExamplePlugin.dll')
    print('Done!')


if __name__ == "__main__":
    main()
