# BasketApi
RESTfull WebAPI based demo to manage an online order's basket items.  
Built using WebApi on top of .Net Framework 4.6, using C#.

### How to Run
- Clone or grab the repo from https://github.com/wrlinden/basketapi
- Open solution file (BasketApi.sln) in Visual Studio (Tested against versions 2015 & 2017)
- Restore Nuget packages for the solution
- Build and Run 
- Run tests with your favourite test runner 
- If running is succesfull, a simple index page linking to the Swagger UI and the generated OpenApi Json will be served up.


### Testing

Testing makes use of the standard MSTest.TestFramework.  
All tests contained in a single class called BasketIntegrationTests.  
Tests test from the produced client inwards through the API 
_(This results in ~100% test coverage, drove behaviour and test driven development but given more time, unit tests would have been introduced against service and repository levels)_

CI Build (AppVeyor - Tests not being detected, ran out of time to fix)  
https://ci.appveyor.com/project/wlinde01/basketapi/


## Specification Assumptions
* Security is dealt with externally, probably at network level (Subnets, SecurityGroups etc.)
* Data model IDs are generated from the outside (for purposes of InMemoryRepository produced)
* Single tenant API
* Async is preferred and implemented (With simulated repository async functions)
* "Integration" testing via the Client is adequate at this stage. 
* Stats and graphs out of scope (For production versions this would not be the case)
* Application logging is out of scope (For production versions this would not be the case)
* Probably has rational db underlying Repo (building something that should fit simple normalised architecture)
* Swagger as documentation and OpenAPI json adequate for endpoint specification
* Assumptions encountered during dev:
  * Nothing sensitive goes into requests, so logging request data outwards is safe
  * Bad Request response (400s) is adequate from the controller endpoints when the service & repository can not complete it's job (like deleting an item that does not exist)
  * Bad request will not be thrown by the controller for Actual exceptions encountered in the stack, those will bubble upwards and ultimately result in Internal Server Error (500s) responses
 

### Api Specs summary:

**Create a new basket**  
_POST /api/Basket_  
Param/body BasketContract  
Returns BasketContract  

**Get basket by Id**  
_GET /api/Basket/\{id\}_  
Returns BasketContract

**Create new item in basket**  
_POST /api/basket/\{id\}/item__  
Param/body BasketContractItem
Returns BasketContractItem

**Delete item from basket**  
_DELETE /api/basket/\{basketId\}/item/\{basketItemId\}__  
Returns 204 

**Delete all items from a basket**
_DELETE /api/basket/\{basketId\}/items__  
Returns 204 

**Increment or decrement Basket Item quantity.**  
_PUT /api/basket/\{basketId\}/item/\{basketItemId\}__  
Param/body _qty to inc/dec_

