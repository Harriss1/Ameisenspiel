# Usage of GitHub Tutorial #
- short list of usefull commands (without explanation):
https://www.nobledesktop.com/learn/git/git-branches
- ein sehr gutes Tutorial habe ich hier gefunden: https://lerneprogrammieren.de/git/

### Simplified steps
After installing Git on your local computer:
1) Create repository on GitHub.com via your own web-account
2) Copy the link from the repository
3) Open Git-Bash inside the desired local folder (set file path via ``cd "filepath"``) -> the destination folder should be empty
4) Command example
`` git remote add origin https://github.com/Harriss1/Ameisenspiel.git``
5) optional: ``git remote -v``
-> maybe add a testfile, for example readme.md

6) ``git push origin main``

7) Copy exisiting project files into local folder

8) ``git add .``

9) ``git commit -m "your message"``

10) ``git push --set-upstream origin main``


-> this was your initial project commit
### later commits work with:
1) ``git add .``
2) ``git commit -m "yourmessage"``
3) ``git push``