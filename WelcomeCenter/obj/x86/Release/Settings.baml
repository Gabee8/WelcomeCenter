   M S B A M L   `   `   `  ����   WelcomeCenter    WelcomeCenter.Settings   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -      settings  � Title#  �   $	��457��$	��630�� � WindowStartupLocation$ CenterOwner=� � UseLayoutRounding .  � Icon$4 ./WelcomeCenter;component/Images/Settings32.png�� � 	Resources  �� Source�� $� �/PresentationFramework.Aero,Version=3.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35,processorArchitecture=MSIL;component/themes/Aero.NormalColor.xaml?�%    .���  � RowDefinitions �� $	��148 ��� �� $��24 � � ColumnDefinitions �� $
��0.5* ��� .+���   programSettings#���  .���� $	��5,0q�.+�	� -   $	��rb0�� 	� 	GroupName$ 
ThemeGroup��  
styleVista#���  
	 � Tag$	 0��	� -   $	��rb1��$ 
ThemeGroup�� 
 style7#���  $	 1��	� -   $	��rb2��$ 
ThemeGroup��  style10#���  $	 2���� �� -   $��sortName��  sort#���  �� -   $��lngCb��
 �� SelectedValuePath$
 Content���� $��1��  appSett#���  .���  �� �� $
��0.2* �.+��� .+���  �� Orientation$ 
Horizontal=�.+��� $��2q�  appName#���  �� -   $��Aname��$	��200���� $ 
Horizontal=�.+��� $��2q�  appPath#���  �� -   $��	Alocation��$	��200���� -	   $��browseAppBt��$	��5,0q�$	��...���� $ 
Horizontal=�.+��� $��2q� 	 	appSilent#��� 	 �� -
   $	��Asw��$	��200���� $ 
Horizontal=�.+���  
 appTitle#��� 
 �� -   $��DescriptName��$	��200���� $ 
Horizontal=�.+���   	appDetail#���  �� -   $��Description��$	��260���� $��1��.+��� -   $��iconImg��$	��0,5q�$��48��$��48���� -   $��saveBt��  VistaCommandButton#��[ $��30��.���� $0��*/WelcomeCenter;component/Images/save24.png���� -   $��addBt��#��[ $��30��.���� $/��)/WelcomeCenter;component/Images/add24.png���� -   $��ListLv��$��1��$��2��   ItemContextMenu m� Icon��%z   &	       �� .@�m� $��MFileDel��-   $��Eltávolítás�� �� $0��*/WelcomeCenter;component/Images/remove.png������  �� 
TargetType" ��.���  �� Property �@�ContextMenu �� Value# [  �� View �� .P��� $
��Név�� �� Width$	 200�� �� DisplayMemberBinding �� *	AppName+�� $��Program helye��$	 280�� �� *
Location+�� $��Silent switch��$	 100�� �� *Silent+�� $��2��$��2��$��0,1,0,0q����	#1E000000._�]� ����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FFFBFDFF�� � Offset$ 0.017Q�� $ 	#FFDDE8F6��$ 1Q�.+��� -   $
��okBt��$	��5,0q�$��0,0,5,0q�$��Right=� 	 apply#���  