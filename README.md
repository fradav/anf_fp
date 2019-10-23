# F# Notebook Presentation
- Install  [latest Dotnet SDK](https://dotnet.microsoft.com/download) for your platform (Mac/Linux/Windows)
- And then,
    - Windows : download and install [Build Tools 2019](https://visualstudio.microsoft.com/fr/thank-you-downloading-visual-studio/?sku=BuildTools&rel=16#), select F# in your workload
    - Linux :
        - Add mono repo as recommanded on [mono project page](https://www.mono-project.com/download/stable/)
        - on Ubuntu/Debian :
        ```plain
        sudo apt-get update
        sudo apt-get install fsharp
        ```
        - on CentOS/RHEL/Amazon/Fedora :
        ```plain
        sudo yum update
        sudo yum install mono-complete fsharp
        ```
    - Macos :
        - [Install mono for mac](https://www.mono-project.com/download/stable/#download-mac)
        - Add `/bin` from Mono to your PATH: 
        ```bash
        export PATH=$PATH:/Library/Frameworks/Mono.framework/Versions/Current/bin/
        ```

- Install [jupyter notebook](https://jupyter.org/install.html), if you haven't done it already.
- Download [latest release of Ifsharp](https://github.com/fsprojects/IfSharp/releases/download/v3.0.3/IfSharp.v3.0.3.zip), the F# kernel for jupyter.
- Unzip it and launch `ifsharp.exe` via mono, if applicable (linux/macos), or directly from windows.
- exit the notebook server

And finally :
- Windows :
    ```cmd
    git clone https://github.com/fradav/anf_fp.git
    cd anf_fp
    jupyter notebook
    ```
- Linux
    ```bash
    git clone https://github.com/fradav/anf_fp.git
    cd anf_fp
    cp local/linux/SQLite.Interop.dll local/
    jupyter notebook
    ```
- Mac
    ```bash
    git clone https://github.com/fradav/anf_fp.git
    cd anf_fp
    cp local/macos/SQLite.Interop.dll local/
    jupyter notebook
    ```

And choose the "F# Introduction.ipynb" file at the jupyter homepage.

The first cell is `paket` initialization, it should take some time the first time.

The `RProvider` part supposes that you have an `R` interpreter in your path.
