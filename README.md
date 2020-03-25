# GameCore

## Game Flow

Seperate to Core and Data.

Core, as Interface, ScriptEnviroment, ModuleManager. Most things need to be set at first of game start.

Data, as Hook, Rule, Card. The game's main data location, could be saved and loaded.
```C#
Core.Init(new INeed());
Core.LoadGame("new game");
```
## Naming Conventions

```c#
class Big
{
	public int Number{ get; set;}
    public float FloatNumber{ get; set;}
    public Concept Concept{ get; set;}
    private int _number;
	internal int numbers;  //no specific
	internal int _numbers; //no specific
    protected Concept _c_concept;
	private int _ctn_concept; //concept's type number
	private int _cdn_card; //card's number
	private Card _cd_card; //card
	public int GetNumber() => _number;
    public void AddNumber(int number)
    {
        _number += number;
    }
    private void _HideFunc(){}
}
```
Define class with inherit `Base.Concept`, add "C" in front of it, ex. `CLocation`, `CLand`.

Define class to be a rule, add "Rule" at its tail, ex. `LocationRule`, `TimeRule`.

Reference of the class `Base.Card`, add "cd_" in front of it, ex. `cd_land`, `cd_tile`.

Reference of the class who inherits `Base.Concept`, add "c_" in front of it, ex. `c_location`, `c_land`

```c#

class TimeRule{}
class LandRule{
    class CLand : Base.Concept
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

`BeNew()`, for Card and its containing concepts who has a only ID, using this to distribute a only ID automatically, sometimes it 
do other things in it.

Most of the time, use the default constructor first, Init() secondly, BeNew() finally.
```c#
var little_card = new Card();
little_card.Init();    //example, actually not like this
little_card.BeNew();   //example.
```

For all Concept and its subclass, use Base.Concept.GetSpawner<T>().Spawn to spawn a instance, T must be a Concept's subclass.

Or use the non-template version: GetSpawner(), put parameter with specific type-number or type-name.

```c#
private CLand _c_land = GameCore.Base.Concept.GetSpawner<GameCore.Root.LandRule.CLand>().Spawn();
private Concept _c_land = GameCore.Base.Concept.GetSpawner<GameCore.Root.LandRule.CLand>().SpawnBase();
private CLocation _c_location = Concept.GetSpawner(c_location_concept_type_number).Spawn() as CLocation;
```

To visit a concept of a `Card`'s instance, use `card.GetConcept()`, and put parameter of the concept type-number. Also you can use the template version `card.GetConcept<T>()`.

```c#
var c_location = location_card.GetConcept(_c_location_type_number) as CLocation; // need transformation
var c_location = location_card.GetConcept<CLocation>();
```

Every Concept inheritance has a only ID, which is distributed automatically at the first instance of concept be 
spawned from the correct way.
> The correct way means using `Concept.GetSpawner()` to Spawn a Concept.

You can get it from static function `GetSpawner()`, or from a instance of concept by using `c_instance.TypeNumber`

```c#
int _c_location_type_number = Concept.GetSpawner<CLocation>().TypeNumber; // by GetSpawner
var c_location = Concept.GetSpawner<CLocation>().Spawn();
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
