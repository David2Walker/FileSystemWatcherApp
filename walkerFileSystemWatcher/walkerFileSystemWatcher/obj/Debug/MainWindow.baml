   M S B A M L   `   `   `  ����R  NwalkerFileSystemWatcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:walkerFileSystemWatcherwalkerFileSystemWatcher  5   	   (    "walkerFileSystemWatcher.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:walkerFileSystemWatcher     5   	     � Title$  File System Watcher��5   	   $	��500��6%   $	��750��62    � 
ResizeMode$ CanMinimize=�6>   .��5	      � .+�5
   
   o�  o� 
IsMainMenu . $��	0,0,0,441q�6!   .@�5      m� -   5         m�Name$ watcherMenuButton��5      $��_File Watcher��62   .@�5      m� -   5        $ startMenuButton��5      $��_Start��64   6D   m� 5      -   5        $ stopMenuButton��5      $��S_top��63   6B   5      m� 5      $��	_Database��6   .@�5      m� -   5        $ queryMenuButton��5      $��_Query��64   6D   m� 5      -   5        $ cleardbMenuButton��5      $��	_Clear DB��66   6I   5      m� 5      $��_Help��6   .@�5      m� -   5        $ aboutMenuButton��5      $��_About��64   6D   5      5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGrid��5      $
��Left=�6%   $	��227��6@   $��	8,211,0,0q�6M   $	��Top=�6`   $	��722��6x      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Watching��6F      
IsReadOnly .    SortDirection$ 	AscendingX�6j   	   Width5  /System.Windows.Controls.DataGridLengthConverter$		 120 6�   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6%   �� *watchingPath+6�     5      $
 User��6B   $	 75 6P    . 
 6%   �� *
userName+6[     5      $ 	File Name��6B   $		 100 6U    . 
 6%   �� *
fileName+6a     5      $
 Path��6B   $		 200 6P    . 
 6%   �� *
filePath+6\     5      $ Event��6C   $	 70 6R    . 
 6%   �� *	fileEvent+6]     5      $
 Time��6>   $	 75 6L    . 
 6%   �� *time+6W     5       $
 Date��6>   $	 75 6L    . 
 6%   �� *date+6W   5"      �� 5#   
   -   5         ��Name$ statusLabel��5#      $��Waiting for start...��6%   $
��Left=�6D   $��
12,435,0,0q�6_   $	��Top=�6s   $	��275��6�   �� 5%   
   -	   5        $ extensionLabel��5%      $(��"Extension to watch (empty for all)��6(   $
��Left=�6U   $��32��6p   $��
214,40,0,0q�6|   $	��Top=�6�   $	��193��6�   �� 5&   
   -
   5         ��Name$ extensionBox��5&      $
��Left=�6)   $��
218,65,0,0q�6D   $	��Top=�6X   $	��120��6p    �� 
IsEditable . $��23��6�   �� 5(   
   -   5        $ watchdirLabel��5(      $)��#Directory to watch        *Required��6'   $
��Left=�6U   $��32��6p   $��7,41,0,0q�6|   $	��Top=�6�   $	��193��6�   �� 5)   
   -   5         ��Name$ watchdirBox��5)      $
��Left=�6'   $��23��6B   $��	12,65,0,0q�6N    �� TextWrapping$
 Wrap=�6a   $	��Top=�6u   $	��179��6�   �� 5*   
   -   5         ��Name$ startButton��5*      $��Start��6&   $
��Left=�66   $��34��6Q   $��
12,163,0,0q�6]   $	��Top=�6q   $��78��6�   6�   �� 5+   
   -   5        $ 
stopButton��5+      $
��Stop��6%   $
��Left=�64   $��34��6O   $��113,163,0,0q�6[   $	��Top=�6p   $��78��6�   6�   �� 5,   
   -   5        $ queryButton��5,      $��Query��6&   $
��Left=�66   $��34��6Q   $��640,163,0,0q�6]   $	��Top=�6r   $��78��6�   6�   �� 5-   
   -   5        $ 
emailLabel��5-      $��Email��6$   $
��Left=�64   $��32��6O   $��	7,100,0,0q�6[   $	��Top=�6n   $	��193��6�   �� 5.   
   -   5        $ emailBox��5.      $
��Left=�6$   $��23��6?   $��
12,124,0,0q�6K   $
 Wrap=�6_   $	��Top=�6s   $	��179��6�   6�   �� 5/   
   -   5         ��Name$ emailcheckBox��5/      $��Send Email Alerts��6*   $
��Left=�6F   $��200,129,0,0q�6a   $	��Top=�6v   $	��150��6�   6�   51      52      