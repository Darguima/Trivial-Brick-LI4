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

Some dependencies are the .NET environment, others for the database. Check the best way to install it on your OS, but here are some generic examples to Arch and Ubuntu.

```bash
# Arch (with yay)
$ yay -S dotnet-sdk aspnet-runtime docker mssql-tools

# Ubuntu
$ sudo apt install dotnet-sdk aspnet-runtime docker mssql-tools # this shouldn't work

# ================

# Both - Configuring docker
$ sudo systemctl enable docker
$ sudo systemctl start docker
$ sudo usermod -aG docker $USER # needs a restart
```

#### Setting up the database

```bash
# Start the database on the docker
$ docker-compose up -d

# Creates and setup database
$ sqlcmd -S localhost,1433 -U sa -P PAss++00 -i setup.sql -No
# You should see - "Everything is ready!"
```

#### Running the project

```bash
$ docker-compose up -d # Start the database
$ dotnet run # Run the project
$ dotnet watch run # Run the project with hot reload
```

## Developed by 🧑‍💻:

- [Afonso Pedreira](https://github.com/afooonso)
- [Dário Guimarães](https://github.com/darguima)
- [Hugo Rauber](https://github.com/HugoLRauber)
- [Rodrigo Macedo](https://github.com/rmufasa)
