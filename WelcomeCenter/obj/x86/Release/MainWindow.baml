   M S B A M L   `   `   `  ����   WelcomeCenter    WelcomeCenter.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -      	maintitle  � Title#  �   $	��650��$	��680�� � Icon$+ %/WelcomeCenter;component/Images/0.ico�� � UseLayoutRounding .  � WindowStartupLocation$ CenterScreen=� � 	Resources  �� Source�� $ styles.xaml?�%    .���  � RowDefinitions �� $	��180 ��� �� $��28 �.+�� -   $��header�� �� $��40 ��� ����  �� ImageSource$3 -/WelcomeCenter;component/Images/VistaHead.png��.+��� -   $��usernamepane��$��40��$	��Top=� �� Orientation$ 
Horizontal=����� $- '/WelcomeCenter;component/Images/104.png��.+��� $��40��$��40��.+��� -   $��sel_IMG��$��24��$��24��$��6q��� $��40��$��40��$
��Left=�$-��'/WelcomeCenter;component/Images/103.png���� 	 J� Effect	 3  -System.Windows.Media.Effects.DropShadowEffect  
   ShadowDepth$
 3Q�   Opacity$	 0.7Q�   Color$ 	#CD000000��._��� -   $��uName��$
��User��$��Center=�
�������$��15(�� $��1�� � ColumnDefinitions �� $	��160 ��� $
��0.9* ��� $��1* �.+��� -    ��Name$ canvas1��.+��� -   $��logoIMG��$
��15,0q�$	��128��$	��128������  �� Path$ ImageSource� �� Mode$ OneTime=��� $��1��	   $
 3Q�$	 0.7Q�$ 	#CD000000��._��� -   $��destPane��.+��� -	   $��descriptionPanel��.+��� -
   $��titleTxt��
�������$��2q�$��15(��� -   $��descriptionTxt��
������� �� Padding$ 2q�$��WrapWithOverflow=��� -   $��systemPropPanel��.+��� -   $��systemEdition��
�������$��2q��� -   $	��cpu��
�������$��2q��� -   $	��ram��
�������$��2q��� -   $	��gpu��
�������$��2q��� -   $��pcname��
�������$��2q�� $��2��.+��� .+��� -   $��runAppBt��  VistaCommandButton#��[ $	��Top=�$��Right=�$��32��.���� $ 
Horizontal=�.+��� $��18��$-��'/WelcomeCenter;component/Images/101.png���� 	   $
 2Q�$	 0.6Q�$ 	#CD000000��._��� -   $��goText��
������� 
 moreBt#���  $��14(�$	��5,0q��� -   $��welcomeText��$��Bottom=�$��	0,0,0,-10q�$��Right=�  welcome#���  
�������$��52(� J� Opacity$	 0.6Q��� -   $��appList��$��10q�$��1��$��0q�$��Disabled=����� $ ListObservableUsers� �� ElementName$ AdminWindow������ .R��  K� IsItemsHost . ���� .R��� $ 
Horizontal=�$	��220��.+��� $��48��$��48��$��2q����� $ Image��� $��5q�$	��170��$��WrapWithOverflow=�$��Center=����� $
 Name��� $��2��$��0,1,0,0q����	#1E000000._�]� ����  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FFFBFDFF�� � Offset$ 0.017Q�� $ 	#FFDDE8F6��$ 1Q�.+��� -   $��
settingsBt��$��24��$��24��#��[ $��0,0,5,0q�$��Right=�.���� $��16��$��16��$:��4/WelcomeCenter;component/Images/settings_list_16.png��