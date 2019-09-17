# Tutoriel général F#
- Installer le [dernier SDK de Dotnet](https://dotnet.microsoft.com/download) pour la plateforme adaptée (Mac/Linux/Windows)
- Ensuite sur chaque plateforme
    - Windows : élécharger et installer les [Build Tools 2019](https://visualstudio.microsoft.com/fr/thank-you-downloading-visual-studio/?sku=BuildTools&rel=16#), en prenant bien soin de choisir le "workload" F#
    - Linux :
        - Ajouter le dépôt mono au système comme indiqué sur [la page du projet mono](https://www.mono-project.com/download/stable/)
        - sur Ubuntu/Debian :
        ```plain
        sudo apt-get update
        sudo apt-get install fsharp
        ```
        - Sur CentOS/RHEL/Amazon/Fedora :
        ```plain
        sudo yum update
        sudo yum install mono-complete fsharp
        ```
    - Macos :
        - [Installer Mono pour mac](https://www.mono-project.com/download/stable/#download-mac)
        - Ajouter le `/bin` de Mono à votre PATH: 
        ```bash
        export PATH=$PATH:/Library/Frameworks/Mono.framework/Versions/Current/bin/
        ```
- Installer [Visual Studio Code](https://vscodium.com/) (version communautaire)
Dans Visual Studio Code installer :
    - Ionide-fsharp

Et enfin :
- Windows :
    ```cmd
    git clone https://github.com/fradav/anf_fp.git
    cd anf_fp
    fake run fsharp_ANF.fsx
    ```
- Linux
    ```bash
    git clone https://github.com/fradav/anf_fp.git
    cd anf_fp
    cp local/linux/SQLite.Interop.dll local/
    ./fake.sh run fsharp_ANF.fsx
    ```
- Mac
    ```bash
    git clone https://github.com/fradav/anf_fp.git
    cd anf_fp
    cp local/macos/SQLite.Interop.dll local/
    ./fake.sh run fsharp_ANF.fsx
    ```

# Démo Web Stack
Pour jouer avec la démo d’un serveur web Full Stack, il faut également avoir :
- [Node.js](https://nodejs.org/en/download/)
- [yarn](https://yarnpkg.com/lang/en/docs/install/)


```bash
git clone https://github.com/CompositionalIT/SAFE-Dojo.git
cd SAFE-Dojo
fake build -t run
```
