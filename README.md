# basketapi
RESTfull WebAPI based demo to manage an online order's basket items.  

CI Build:  
https://ci.appveyor.com/project/wlinde01/basketapi/



## ToDo / POA
* Introduce blank WebApi project 
* Introduce simple health check
* Work out RESTfull specification
* Build out Contract / Model (keep simple, 1-2-1 mapping)
* Build out endpoints
  * Do one single vertical function to introduce a full vertical (test/client/controller/service/repo)
    * Probably create or get basket item
    * Test from client inwards through API 
  * Repeat for each endpoint / function required
* Do any final refactoring
* Time permits - Wire in AWS CodeDeploy/Pipeline or AppV or such
* Documentation if needed

## Assumptions
* Security is dealt with externally, probably at network level (Subnets, SecurityGroups etc.)
* Data model IDs are generated from the outside (for purposes of InMemoryRepository produced)
* Single tenant API
* Async is preferred
* "Integration" testing via the Client is adequate at this stage. 
* Stats and graphs out of scope 
* Logging optional at this stage - (might add a filter based quick solution - time permits) 
* Probably has rational db underlying Repo (building something that should fit simple normalised architecture)
* Swagger documentation Adequate 
* Assumptions encountered during dev:



### Api Specs:

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

