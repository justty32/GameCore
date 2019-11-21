# GameCore

## Naming Conventions

Most Conventions are reserved from C#'s.

```c#
class Big
{
    public int Number{ get; set;}
    public float FloatNumber{ get; set;}
    public Component Component{ get; set;}
    private int _number;
    protected Component _c_component;
    public int GetNumber() => _number;
    public void AddNumber(int number)
    {
        _number += number;
    }
    private void _HideFunc(){}
}
```
Class who inherit `Base.Component`, add "C" at its front, ex. `CLocation`, `CLand`

Class who is a rule, add "Rule" at its tail, ex. `LocationRule`, `TimeRule`

Reference of the class `Base.Card`, add "_card" at its tail, ex. `land_card`, `tile_card`

Reference of the class who inherit `Base.Component`, add "c_" at its front, ex. `c_location`, `c_land`

```c#

class TimeRule{}
class LandRule{
    class CLand : Base.Component
    {
        public int num;
    }
    private CLand _c_land_instance;
    protected Card _land_card;
}
```

## Usage Habits

For all Component and its inheritance, use Base.Component.GetSpawner<T>().Spawn to spawn a instance, T must be a Component inheritance.

Or use the non-template version: GetSpawner(), put parameter with specific type-number or type-name.

```c#
private CLand _c_land = GameCore.Base.Component.GetSpawner<GameCore.Root.LandRule.CLand>().Spawn();
private Component _c_land = GameCore.Base.Component.GetSpawner<GameCore.Root.LandRule.CLand>().SpawnBase();
private CLocation _c_location = Component.GetSpawner(c_location_component_type_number).Spawn() as CLocation;
```

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
