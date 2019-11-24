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
Define class with inherit `Base.Component`, add "C" in front of it, ex. `CLocation`, `CLand`

Define class to be a rule, add "Rule" at its tail, ex. `LocationRule`, `TimeRule`

Reference of the class `Base.Card`, add "_card" at its tail, ex. `land_card`, `tile_card`

Reference of the class who inherits `Base.Component`, add "c_" in front of it, ex. `c_location`, `c_land`

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

For most class, it define three functions for different purpose of initialize, default constructor, `Init()`, `BeNew()`.

Default constructor, for `new` a pure instance, mostly it contains nothing, and being public.

`Init()`, it replace the things put in default constructor originally, in order to control the order of initializing for instances, 
ex. allocating memory, setting the intial number...

> For variables xx_number_distribute_reference, which is a reference for distribute numbers of something, using example:
```c#
public void BeNew(){
    //reference number is from Core.Instance mostly 
    thing_number = reference_number;  
    reference_number++;
}
```

`BeNew()`, for Card and its containing components who has a only ID, using this to distribute a only ID automatically, sometimes it 
do other things in it.

Most of the time, use the default constructor first, Init() secondly, BeNew() finally.
```c#
var little_card = new Card();
little_card.Init();    //example, actually not like this
little_card.BeNew();   //example, it is BeNewCard() actually.
```

For all Component and its subclass, use Base.Component.GetSpawner<T>().Spawn to spawn a instance, T must be a Component's subclass.

Or use the non-template version: GetSpawner(), put parameter with specific type-number or type-name.

```c#
private CLand _c_land = GameCore.Base.Component.GetSpawner<GameCore.Root.LandRule.CLand>().Spawn();
private Component _c_land = GameCore.Base.Component.GetSpawner<GameCore.Root.LandRule.CLand>().SpawnBase();
private CLocation _c_location = Component.GetSpawner(c_location_component_type_number).Spawn() as CLocation;
```

To visit a component of a `Card`'s instance, use `card.GetComponent()`, and put parameter of the component type-number. Also you can use the template version `card.GetComponent<T>()`.

```c#
var c_location = location_card.GetComponent(_c_location_type_number) as CLocation; // need transformation
var c_location = location_card.GetComponent<CLocation>();
```

Every Component inheritance has a only ID, which is distributed automatically at the first instance of component be 
spawned from the correct way.
> The correct way means using `Component.GetSpawner()` to Spawn a Component.

You can get it from static function `GetSpawner()`, or from a instance of component by using `c_instance.TypeNumber`

```c#
int _c_location_type_number = Component.GetSpawner<CLocation>().TypeNumber; // by GetSpawner
var c_location = Component.GetSpawner<CLocation>().Spawn();
_c_location_type_number = c_location.TypeNumber;   // by instance.TypeNumber
```

## Develop Habits

For every `Rule` Class Defining, it should (not must) contains two members, `Init()`, default constructor.

Because while initializing the `Core.Instance`, some rule instances has been new. If there are some rule's default constructor 
needs to visit the `Core.Instance`, it may cause errors.

So, while initializing the `Core.Instance`, there is just new a rule's instance by default constructor. After it, it doing `Rules`'s Init().

---

Only `Has()` and `Is()` function return literally value

Other `Check`, `Add()`, `Remove()`, `Init()`, `Sub()`. `AutoSet`, `Set`, comply with below rules:
>return false, if process is good at all
>return true, if there is any error on process, ex. illegally parameter.
