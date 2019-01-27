# basketapi
RESTfull WebAPI based demo to manage an online order's basket items.

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
* Time permits - Wire in AWS CodeDeploy/Pipeline or AppV or such
* Documentation if needed

## Assumptions
* Security is dealth with externally, probably at network level (Subnets, SecurityGroups etc.)
* Single tenant API
* Stats and graphs out of scope 
* Logging optional at this stage - (might add a filter based quick solution - time permits) 
* Probably has rational db underlying Repo (building something that should fit simple normalised architecture)
* Swagger documentation Adequate 
* Assumptions encountered during dev:
  * 