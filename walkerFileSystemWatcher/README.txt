Known Issues: 
- Events can fire twice or more for "one" change
	- ex) creating a file fires a create event and a change event for one action
	- From my research this is a system bug/feature related to the way the Windows saves files with multiple system calls
	- Making the Watcher avoid this can be difficult from what I've read and since there's no real consequence I decided to focus on other tasks
	- more information:
		http://stackoverflow.com/questions/1764809/filesystemwatcher-changed-event-is-raised-twice
		http://weblogs.asp.net/ashben/31773
	

Description/Features:
File System Watcher
-allows the user to monitor changes (Deletion, Creation, Change, Renaming) of files and directories of a specified path
-uses threading to dynamically update table as changes happen

Option for email alerts
-To enable email alerts, check the "Send Email Alerts" Box and enter the email address you'd like to recieve alerts at
-To avoid spamming the user every time a change happens, after first event there is a running timer which is reset each time a new event happens
	-it's only when this timer finishes that an email is sent out to the specified address 

Query Window
-Access via the Query button on the window or in the menu strip
-Opens second window that allows the user to query the databse for specific file names or extensions or simply the whole database
	-Secondary sort options are available via the table itself where you can sort each column individually 

