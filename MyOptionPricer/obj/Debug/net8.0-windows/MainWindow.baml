   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   I EMyOptionPricer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:MyOptionPricerMyOptionPricer 5   	      MyOptionPricer.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   .localclr-namespace:MyOptionPricer     5   	     � Title$  OptionPricer��5	   	   $	��950��5
   	   $	��616��5   	    � Icon$ /Content/Icon.ico��5   	    � WindowStartupLocation$ CenterScreen=�5   	   ��5      
� 5   
    
� Center$ 0.5,0.5&�6    
� GradientOrigin$ 0.5,0.5&�6/    
� RadiusX$ 1Q�6H    
� RadiusY$ 1Q�6T   .��5      �  � Color$ #8C48BA��6    � Offset$	 0.0Q�6+   � 5      $ #C37B9B��6   $	 0.5Q�6+   � 5      $ #FAAE7B��6   $	 1.0Q�6+   5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $
��0.2* �6   �� 5      $
��0.8* �6   .+�5!   
   �� $��0��6   
������ ._�5"      �� $��60(�6   $��My Option Pricer��5#      $��Center=�5$      $��Center=�5%      $��High Tower Text)�5&      ���#c2b6475'      5(      �� 5+   
   $��1��6   $��10q�6"   ��5-      �� 5.      
 �� Color$
 	#FFE0E0E0��6"    �� Opacity$	 0.6Q�64    J� OpacityMask 51      !� 52       !� Visual 53      � 54      $��30��6   $��30��6*   .+�55      �� ��56   "   L� 57   &    j� TileMode$
 Tile=�63    j� Viewport$ 0,0,0.1,0.1�6C    j� ViewportUnits$ RelativeToBoundingBox=�6Z    L� Drawing 58   *   I� 59   .   .��5:   2   �  � Brush
 �  �� � Geometry 5;   6   �� 5<   :    �� Rect$ 	0,0,10,10�6L   5>   3   � 5?   2   
 ����  5@   6   � 5A   :   .��5B   >   @�  @� Center$	 2,2&�6N    @� RadiusX$ 1Q�6[    @� RadiusY$ 1Q�6g   @� 5C   >   $	 7,7&�6N   $ 1Q�6[   $ 1Q�6g   @� 5D   >   $	 3,8&�6N   $	 0.8Q�6[   $	 0.8Q�6i   @� 5E   >   $	 8,3&�6N   $	 0.8Q�6[   $	 0.8Q�6i   5F   ;   5H   3   5I   /   5K   '   5M      5N      5P       � 	Resources 5S       �� 
TargetType �� Property �� Value �� 
TargetType �� Padding �� CornerRadius ��Name  �� Triggers! P� Property" P� Value# �� 
TargetName��%�  '��       �� 5T      " ��6   .�5U      ��  � �� 5V      �� 5W      " ��6.   .R�5X   "   �� $��Border��6)   #��z��5Y   )   $��1q�5Z   )   
������ $
 15 5q�5\   )   $ 	10,10,0,0��5]   )   ._�5^   &   �� $ ContentSite��67   $��Center=�5_   7   $��Center=�5`   7   $��Header��5a   7   5b   #     5c   "   P� 5d   &   ! �@J�IsMouseOver$
" True��6E   . �5e   *   �� $# Border��61    � �� �#e4c3e96[   5f   '   P� 5h   &   ! �@��
IsSelected$
" True��6D   . �5i   *   �� $# Border��61    � �� �#e4c3e96[   5j   '   5l      5n      �� 5o       � ��$ 18(�61   �� 5p       � ��$
 15 5q�60   5q      5r      .@�5t      �� $��European��6   ���	#AAFFFFFF.��5u      �� .��5v      �� $��20q�6!   .+�5w      �� $��European Option��6$   $��24(�6;     MyOptionPricer.EuroForm  5x      5y      5z      5{      �� 5}      $��American��6   ���	#AAFFFFFF.��5~      �� .��5      �� $��20q�6!   .+�5�      �� $��American Option��6$   $��24(�6;     MyOptionPricer.AmerForm  5�      5�      5�      5�      �� 5�      $��Asian��6   ���	#AAFFFFFF.��5�      �� .��5�      �� $��20q�6!   .+�5�      �� $��Asian Option��6$   $��24(�68     MyOptionPricer.AsianForm  5�      5�      5�      5�      5�      5�      5�      