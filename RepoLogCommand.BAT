cd C:\Users\CTDragon\Desktop\Google\SFSO Project\SFSO
(hg log -v -d "jan 2014 to jan 2014" --template "{date|isodate}\r\nDescription: {desc}\r\n\r\n") | sed ':a;N;$!ba;s/\n/\\r\\n/g' >> RepoLog.txt

pause