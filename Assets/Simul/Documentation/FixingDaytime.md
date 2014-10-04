Fixing Time-of-Day
------------------

Sometimes, you might not want a day-night cycle or changing time-of-day, but still want the clouds to move and weather to change over time. To do this, you can uncouple keyframe time (the position of a keyframe on the timeline) from daytime.

Edit the Sequence asset by clicking on it in the Project window. Select the Sky (click on the word "Sky" at lower left) and uncheck "*Link Keyframe time and daytime*".

![](http://simul.co/wp-content/uploads/documentation/unity/FixingDaytime1.jpg)

Now, select all the Sky keyframes (box-select or double-click). Under "Sun and Moon", change the "daytime" of the keyframes to be the same value (e.g. 06:45 below). Now time and time-of-day are uncoupled.

![](http://simul.co/wp-content/uploads/documentation/unity/FixingDaytime2.jpg)