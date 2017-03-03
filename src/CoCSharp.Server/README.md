# Setup

### Requirements
 * A `MySQL server` for storage of user accounts and other stuff running somewhere.
 * An `HTTP server` for patching of client assets running somewhere.
 * Compiled `CoCSharp` solution.

## Database Setup
At the moment `CoCSharp.Server` only supports `MySQL` as database provider, so you must set up a `MySQL server` running
somewhere.

You must then create the `MySQL` database and tables using

```bash
mysql> source CoCSharp.Server\contents\cocsharp.mysql
```

This will create the database and all the tables needed for the server to run and store accounts and other stuff.

---

## Patching Setup
The patching server is simply an `HTTP server` that hosts assets files which client which outdated content downloads, the server
too downloads the assets when it starts up.

```dir
└───6552acaf9688069c8e26f61b8089e8da0fc7b01a
    ├───csv
    ├───font
    ├───image
    ├───level
    │   └───test
    ├───logic
    │   └───out
    ├───music
    ├───sc
    └───sfx
        ├───caf
        └───ogg
```

When the server will start up, it will download all the assets at the specified `content_url` and `master_hash`.

If you want to skip this part you can use the official Supercell patching servers for patching which is located at 
`http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/`. However to keep up with latest assets you need
to continously update the `master_hash` located in the config file with latest one.

---

## Configurations
 All configurations of the server such as starting gold, starting elixir and etc are stored in `server_config.xml`.
 
 If you're running the server for the first time, the server will create the `server_config.xml` file with the default
 configurations. However it won't create the `mysql_host`, `mysql_user` and `mysql_pwd` fields to the config.

#### Database configurations
 `mysql_host` points to the the host of the `MySQL server` instance.<br>
 `mysql_user` is the username the server will use to login.<br>
 `mysql_pwd` is the password the server will use to login.

 To make the server to connect to the database you need to add the `mysql_host`, `mysql_user` and `mysql_pwd` fields
 to the XML config file manually with correct values of course.

---

#### Content configurations
 `content_url` URL pointing to the host of the `HTTP server ` for patching of assets.<br>
 `master_hash` master hash of the content.

---

#### Gamplay configurations
 `starting_gems` amount of gems a new account starts with.<br>
 `starting_gold` amount of gold a new account starts with.<br>
 `starting_elixir` amount of elixir a new account starts with.<br>

---

#### Example `server_config.xml`
```xml
<?xml version="1.0" encoding="utf-8"?>
<server_config>
  <starting_gems>10000000</starting_gems>
  <starting_gold>100000000</starting_gold>
  <starting_elixir>100000000</starting_elixir>
  <content_url>http://192.168.100.7/</content_url>
  <master_hash>2f2c3464104feb771097b42ebf4dfe871bd56062</master_hash>
  <mysql_host>localhost</mysql_host>
  <mysql_user>root</mysql_user>
  <mysql_pwd>123456</mysql_pwd>
</server_config>
```

---

## Windows
 1. Configure the `server_config.xml`.
 2. Run `CoCSharp.Server.exe`.

## Linux
 1. Build `libsodium`.
 2. Place the compiled libsodium binary to root directory of the server and
    make sure the file is named `libsodium`.
 3. Configure the `server_config.xml`.
 4. Run `mono CoCSharp.Server.exe`.
#### Note
It was only tested on a Ubuntu machine.