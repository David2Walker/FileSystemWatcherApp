   M S B A M L   `   `   `  ����R  NwalkerFileSystemWatcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:walkerFileSystemWatcherwalkerFileSystemWatcher  5   	   %    walkerFileSystemWatcher.Window1   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:walkerFileSystemWatcher     5   	   -   5          �Name$  queryWindow��5   	    � Title$ Query��5   	   $��382.312��6   $��763.062��6(    � 
ResizeMode$ CanMinimize=�68   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ 	queryGrid��5
      $
��Left=�6&   $��	10,70,0,0q�6A   $	��Top=�6T   $	��735��6l   $	��271��6x      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Watching��6F      
IsReadOnly .    SortDirection$ 	AscendingX�6j      Width5  /System.Windows.Controls.DataGridLengthConverter$	 125 6�   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6%   �� *watchingPath+6�     5      $
 User��6B   $ 65 6P    . 	 6%   �� *
userName+6[     5      $ 	File Name��6B   $	 100 6U    . 	 6%   �� *
fileName+6a     5      $
 Path��6B   $	 200 6P    . 	 6%   �� *
filePath+6\     5      $ Event��6C   $ 75 6R    . 	 6%   �� *	fileEvent+6]     5      $
 Time��6>   $ 75 6L    . 	 6%   �� *time+6W     5      $
 Date��6>   $ 75 6L    . 	 6%   �� *date+6W   5      �� 5   
   -   5        
 ��Name$
 queryforLabel��5      $"��Enter filename or extension:��6'   $
��Left=�6N   $��	10,23,0,0q�6i   $	��Top=�6|   �� 5   
   -   5         ��Name$ queryforBox��5      $
��Left=�6(   $��
172,24,0,0q�6C   $	��Top=�6W   $	��128��6o    �� 
IsEditable . $��23��6�   �� 5   
   -   5         ��Name$ queryButton��5      $��Query DB��6&   $
��Left=�69   $��
361,19,0,0q�6T   $	��Top=�6h   $��95��6�   $��30��6�   6�   5      5      