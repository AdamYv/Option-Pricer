   M S B A M L   `   `   `  ����I  EMyOptionPricer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:MyOptionPricerMyOptionPricer  5          MyOptionPricer.AmerForm   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:MyOptionPricer     5        � 	Resources  5	         PlaceholderTextBox �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Path �� RelativeSource �� Mode	 �� UpdateSourceTrigger
 ��Name
 � Tag �� Triggers P� Property P� Value �� 
TargetName��%\  &	        �� 5   
   " ��6+   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ textBox��6&   #��z��5   &   #��z��5   &   #��z��5   &   #��z��5   &   $��20(�5   &   ��5   &   �� $
 Text� �� *TemplatedParent+$ TwoWay=�$	 PropertyChanged=�5   &   �� 5      $
 placeholder��6(   #��z 5   &   
�������$��5,2,0,0q�5   &   $y�	Collapsed=�5   &   5       5      P� 5       � ��$  ��66   . �5    "   �� $ placeholder��6?    � y�$ Visible=�6X   5!      5#      5%      5&      5'      .��5)      �  � RowDefinitions 5+   
   �� 5,      $
��Auto �6   �� 5-      $
��Auto �6   .+�51   
   � $��0��6   $��20q�6    � ColumnDefinitions 52      �� 53      $��* �6#   �� 54      $��* �6#    56      �� 57      $
��Auto �6    �� 58      $
��Auto �6    �� 59      $
��Auto �6    �� 5:      $
��Auto �6    .+�5>      �� $��0��6   $��0��6)   $��10q�66   .+�5?      �� $��Strike Price��6   $��20(�6/   �� 5@      -   5        $ 	txtStrike��5A      #��[  5@      $
��Left=�5B      $	��200��5C      $ Enter strike price��5D      $��14(�5E      $��0 0 0 10q�6(   5F      �� 5G      $��1��6   $��0��6)   $��10q�66   .+�5H      �� $��
Spot Price��6   $��20(�6-   �� 5I      -   5        $ txtSpot��5J      #��[  5I      $
��Left=�5K      $	��200��5L      $ Enter spot price��5M      $��14(�5N      $��0 0 0 10q�6(   5O      �� 5Q      $��0��6   $��1��6)   $��10q�66   .+�5R      �� $��Risk-Free Rate (%)��6   $��20(�65   �� 5S      -   5        $ txtRiskFree��5V      #��[  5S      $
��Left=�5T      $	��200��5U      $ Enter annual rate��5W      $��14(�5X      $��0 0 0 10q�6(   5Y      �� 5Z      $��1��6   $��1��6)   $��10q�66   .+�5[      �� $��Volatility (%)��6   $��20(�61   �� 5\      -   5        $ txtVolatility��5_      #��[  5\      $
��Left=�5]      $	��200��5^      $ Enter volatility��5`      $��14(�5a      $��0 0 0 10q�6(   5b      �� 5d      $��0��6   $��2��6)   $��10q�66   .+�5e      �� $��Time to Maturity (years)��6   $��20(�6;   �� 5f      -   5        $ txtMaturity��5i      #��[  5f      $
��Left=�5g      $	��200��5h      $ Enter time period��5j      $��14(�5k      $��0 0 0 10q�6(   5l      �� 5o      $��1��6   $��2��6)   $��10q�66   .+�5p      �� $��Accuracy��6   $��20(�6+   �� 5q       �� Orientation$ 
Horizontal=�6   $��Center=�66   .+�5s      �� -   5         ��Name$ sliderAccuracy��5s       � Minimum$ 1Q�5t       � Maximum$
 1000Q�6    �� TickFrequency$ 10Q�5u       �� IsSnapToTickEnabled$
 True��5v       � Value$ 1Q�5w      $	��150��5x      $��0 0 10 0q�5y      �� 5{      -   5        $
 txtAccuracyValue��5{       $��14(�5}      $��Center=�5~      ��5|      �� *Value+ �� ElementName$ sliderAccuracy��5~      5      5�      �� 5�      $��0��6   $��3��6)   $��10q�66   .+�5�      �� $��Dividend��6   $��20(�6+   �� 5�      -   5        $ txtDividend��5�      #��[  5�      $
��Left=�5�      $	��200��5�      $# Enter your dividends per year��5�      $��14(�5�      $��0 0 0 10q�6(   5�      �� 5�      $��1��6   $��3��6)   $��10q�66   $ 
Horizontal=�6B   .+�5�      �� $ Vertical=�6   $
��10,0q�64   .+�5�      �� $�� Call ��6   $��20(�6-   $��Center=�6;   	� 5�      -	   5         	�Name$ 	radioCall��5�   3    d� 	IsChecked$
 TrueY�6"   $��Center=�6F    	� 	GroupName$ 
OptionType��6c   5�      �� 5�      $ Vertical=�6   $
��10,0q�64   .+�5�      �� $�� Put ��6   $��20(�6,   $��Center=�6:   	� 5�      -
   5        $ radioPut��5�   "   $��Center=�64   $ 
OptionType��6Q   5�      5�      5�      �� 5�   
   $��1��6   $ Vertical=�6"   $��Center=�69   $
��0 20q�6V   .+�5�      �� $ Vertical=�6   $��Center=�60   .+�5�      �� $
��Bold"�6   $��20(�6.   Price6D   �� 5�      -   5        $
 	txtResult��5�   G   $��Center=�6   $��20(�69   6_   5�      �� 5�      $ 
Horizontal=�6   .+�5�      �� -   5        $��Compute��5�      $��20(�6'   $��10q�65   $
��10 5q�6A   6P   �� 5�      -   5        $��Copie��5�      $��20(�6%   $��10q�63   $
��10 5q�6?   6N   5�      5�      5�      5�      