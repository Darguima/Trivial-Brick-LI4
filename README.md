# Trivial Brick - LI4

## About the Project

This project was developed for the subject LI4 from University of Minho - Software Engineering degree.

#### Grade ⭐️ ⚠️ Change me ⚠️/20

### Demo 📽️ ⚠️ Change me ⚠️

![Demo Image](./readme/_.png)

⚠️ Change me ⚠️ link to video

### The goal ⛳️

* ⚠️ Change me ⚠️ 

### About the Code 🧑‍💻

* ⚠️ Change me ⚠️ 

## Getting Started 🚀

#### Cloning the repository

```bash
$ git clone git@github.com:Darguima/Trivial-Brick-LI4.git
$ cd Trivial-Brick-LI4
```

#### Installing system dependencies

Some dependencies for the project are the .NET SDK, ASP.NET Core and docker. Check the best way to install it on your OS, but here are some generic examples to Arch and Ubuntu.

```bash
# Arch
$ sudo pacman -S dotnet-sdk aspnet-runtime docker mariadb

# Ubuntu
$ sudo apt install dotnet-sdk aspnet-runtime docker mariadb

# ================

# Both - Configuring docker
$ sudo systemctl enable docker
$ sudo systemctl start docker
$ sudo usermod -aG docker $USER  # needs a restart
```

#### Setting up the database

```bash
# Start the database on the docker
$ docker-compose up -d

# Test it
$ mariadb -h 127.0.0.1 -u blazoruser -p # password: blazorpass

> SHOW DATABASES;

# Create database
$ mariadb -h 127.0.0.1 -u blazoruser -p < database/setup.sql
```

#### Running the project

```bash
$ docker-compose up -d # Start the database
$ dotnet run # Start the project
```

## Developed by 🧑‍💻:

- [Afonso Pedreira](https://github.com/afooonso)
- [Dário Guimarães](https://github.com/darguima)
- [Hugo Rauber](https://github.com/HugoLRauber)
- [Rodrigo Macedo](https://github.com/rmufasa)
