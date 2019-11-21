# GameCore

for all protected or private variable member, add "_" before it, ex. _c_location.

For a class who inherit Base.Component, it need to named C + "name".
ex. location component is named 'CLocation'.
And, for its reference variable, is named c_ + "name"". 
ex. location component variable public member is named 'c_location'

For all Component inherition class, use Base.Component.GetSpawner<>() to spawn a instance

for xx_number_distribute_reference, which is a reference for distribute numbers
mostly using example: 
'''c++
thing_number = get_reference_number;
reference_number++;
'''

for every rule classes, needs to made a default constructor and a Init() function
that's cause while Core Instance initializing, needs to new instances for these rule classes
, on this state, Core Instance use default consuctor to new it,
so, it can visit the Core Instance while rule classes's Init().

use BeNew() to be a new Card, CLocation....
only Has(), Is() function return literally value
other Add(), Remove(), Init(), Sub(), comply with below rules:
return false, if process is good at all
return true, if there is any error on process, ex. illegally parameter.