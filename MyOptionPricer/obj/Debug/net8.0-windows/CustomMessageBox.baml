   M S B A M L   `   `   `  ����I  EMyOptionPricer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:MyOptionPricerMyOptionPricer  5   	   %    MyOptionPricer.CustomMessageBox   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:MyOptionPricer     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ CustomMessageBox��5	   	   $	��300��5
   	   $	��500��5   	   ��5      �� 5   
    �� 
StartPoint$	 0,0&�6    �� EndPoint$	 1,1&�6/   .��5      �  � Color$ Orange��6    � Offset$	 0.0Q�6*   � 5      $ 	OrangeRed��6   $	 0.5Q�6-   � 5      $	 Red��6   $	 1.0Q�6'   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $
��Auto=�5      $��Disabled=�5      $��Stretch=�5      $��Stretch=�5      .��5      �� -   5         ��Name$ 
txtMessage��5       �� TextWrapping$
 Wrap=�5       $��20(�5!      
�������$��Center=�5#      $��Center=�5$      
������ $��0q�5&      	 �� 
IsReadOnly$
	 True��5'      
 �� TextAlignment$
 Center=�5(      5)      �� 5+   
   $��1��6    �� Orientation$ 
Horizontal=�5,      $��Center=�5-      $��	0,10,0,15q�5.      .+�5/      �� -   5        $	��100��5/      $��25��50      $��Ok��51      $��15(�52      $	��5,0q�53      ��55      �� 56       �� 
TargetType" ��6&   .R�57      �� 
������ �� CornerRadius$ 10��58   !   ._�59      �� $��Center=�6/   $��Center=�5:   -   5;      5<      5>      �� 5@      -   5        $	��100��5@      $��25��5A      $
��Bold"�5B      $��Copier��5C      $��15(�5D      $	��5,0q�5E      ��5G      �� 5H      " ��6&   .R�5I      �� 
���q�<�$ 10��5J   !   ._�5K      �� $��Center=�6/   $��Center=�5L   -   5M      5N      5P      5Q      �� 5R   
   -   5         ��Name$ txtNotification��5R      $+��%Texte copié dans le presse-papiers !��5S      $��Center=�5T      $	��Top=�5U      $
��0,20q�5V       �� Padding$
 15,8q�5W      ���	#FF81C784
�������$��14(�5Z       �� TextAlignment$ Center=�5[      $y�	Collapsed=�5\       K� ZIndex$	 999��5]       J� RenderTransformOrigin$ 0.5,0.5&�5^      z�5_      T� 5`       T� Y$	 -20Q�6%    J� Effect 5b      3  -System.Windows.Media.Effects.DropShadowEffect  5c         
BlurRadius$ 10Q�6#      Color$ Black��5d   !      Opacity$	 0.3Q�5e   !      ShadowDepth$ 2Q�5f   !   5h      5m      5n      