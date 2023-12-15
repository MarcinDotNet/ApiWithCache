# ApiWithCache

How to run:

 In visual studio = run project AspWIthCacheApi

 Swagger page should open.
 sample query: https://localhost:7290/HackerNews/bestStories/40

 Assumptions:
  - data on hackerrank service is not changing to often 
  - initial data load is not to big
  - service users can wait a little until data will be accessible
  - not disturbing external server perfomance is a priority
  - easy extension to with different external services is needed
  - each compoenent should be easy to change
  - no memory leak,
  - short time = simple but stable solution
  - in the SimpleStoryDataService only data in the  _dictionaryOfProviderResults will be changing and there will be race 
  
  How it works:

  At start of the program the data from external service is loaded. When all data is loaded, then value is inserted in dictionary. Every x minutes for every provider data is loaded fully again new list is inserted into dictionary. Quick action. Data in dictionary cannot be changed- only full list can be updated. All new reuqest will process with a new list.


 Controller errors are specified in the cs file.

 What next:

 This is one server solution. So it should use some inmemory/db cache  like ie. redis.
 App should be divided in 2 components
    - one writing to redis.
    - second serviceing wep api content.
    
Other thing is how to speed up the reading data.
We can load a list of all stories to process, then refresh this stories in parraller ie.
