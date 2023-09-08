# If-Else Replacement Pattern
### For OO conditional branching that meets the criteria: open-closed principle
As we all know, if-else branching construct is indispensable for programming. . For small branching conditions this is ideal, but for many if-else cases like hundreds, it could be unwieldy. Switches can only handle characters or specific integer values, of which is the result of the expression condition, e.g., 
```
Switch(expressionToValue){
Case ‘A’: dosomething(); break;
Case 1: doanotherthing(); break;
}
```
You can’t do this on Switch():
```
Switch(name){
Case “Flanders”: thisIsNotPossible(); break;
}
```
What if I can show you a replacement for if-else conditional branching that can handle **anything**? Though, you have to code that “anything” into objects, using a blend of template &amp; chain-of-responsibility design pattern...

![image](https://github.com/josephedwardchang/If-Else-Replacement-Pattern/assets/21256796/c2660b5e-38c6-480a-bf53-bb42b4bbb360)
Screenshot above is UML and Sequence diagram of the topic. But it doesn’t show the true subtle inner workings as codes does, it was meant to be an overview.

#### Details of the UML diagram
MainObj is derived from the abstract class and uses initializeChain() that contains the successor values for each of the objects. That includes creating Obj1..Obj4 and setting Obj1.successor = Obj2 then Obj2.successor = Obj3 and so on. MainObj then uses Execute(param) to start operations on param. Param is a reference to another object that needs to be manipulated commonly by the Objxx classes. For example, we can use the param object as an empty person form (which is pre-filled to include the filename) to be filled by various file formats being parsed by Objxx classes. Suppose Obj1 handles json format, Obj2 handles XML and so on. It is easy to see that we can extend (Open-closed Principle) this Design Pattern (I call it If-Else Replacement pattern) to easily add, say, handling CSV files. Just change the initializeChain() to include (and create) new Obj5 class and set the successor from the last successor in the chain [Obj4.successor = Obj5 (that handles CSV files)]. 

Each Objxx class implements and overrides 2 abstract methods: 
1)	CanHandle() – returns Boolean value to indicate if Objxx can handle the incoming param object filename. For this handler, it can perform checking the param object to see if the message is indeed of the type that it can handle and parse.
2)	HandleIt(Object param) – this is the function where the action of the object goes into. In our example this will contain the parsing actions. Param is then filled up when the function returns.

#### Details of the MainObj class
Did I whet your appetite to see the codes yet? Or did you just go straight into diving through the codes? Go take a look while reading the info below:

- Inside InitializeChain() method, you have 2 steps: create the handler objects (Obj1..4) and 2) set each of their successors one after the other. Warning though, not to make the succession in a circular loop and not make any handler be skipped, otherwise that’s a bug right there! 

- Inside Execute(Object param) method, it calls the actual method “HandleRequest()” that has the logic to determine if it can call on the “HandleIt()” method for all the objects in the chain. The way it does this is: to get the result of the CanHandle() for each of the Objects and if it is true and it executes the HandleIt() of that Object. If it throws error or CanHandle() returns false, it moves to the next object and if it done, it returns all the way to MainObj.Execute() finishing the method.

- Inside each of the defined Handler Objects (see MsgHandlers.cs file), you would see they defined their own CanHandle() and HandleIt() methods according to their singular purpose (parse json or parse GPS, etc). 

Now you can trace how this design pattern works by debugging through the codes, yeah?! Of note is how the HandleRequest() performs “calling” the next handler in chain…it uses if-else construct!!! _Isn’t it ironic, don’t you think?_

#### Limitations
All the handler objects are running while MainObj lives, as such we can defer handler object creation by using DI (Dependency Injection) principles (or make them IDisposable) but it’s a bit complicated it would just clutter the pattern. Of course, when all objects are in memory, that’s a waste -- since the pattern just calls each handler sequentially, anyway. I would start improving this when I learn the publish-subscribe pattern.

But that would also introduce problems in and of itself, yes? Like, what if we continually call Execute() on a big handler object that’s always creating/destroying itself whenever it is called upon or not used anymore? Big performance drop, I’d say.

This abstract class can be changed by providing return value to indicate success or failure. Or we can change the MsgData class being passed to the handler objects (the param object passed into Execute method) to include a Boolean property to indicate success/failure. In the example code, I used MsgData.Filename to indicate the handler has changed it and was indeed successful in parsing it.

This pattern solves the problem when 1 handler is all that is needed to be called upon meaning, the other handlers in the chain would not execute once a handler gets called and handled it. Well, what if we want multiple handlers on the chain to have a go? Now, that’s a question for inquisitive minds and advanced devs, eh? Good luck, I leave it up to those curious folks. _FYI, I know the solution and it takes only 1 keyword to be removed..._

## Output of code sample
![image](https://github.com/josephedwardchang/If-Else-Replacement-Pattern/assets/21256796/4d25dfec-3580-419e-a52d-933f727301c7)
