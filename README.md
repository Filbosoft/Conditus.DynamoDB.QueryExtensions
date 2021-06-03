# Conditus DynamoDB Query extensions
A NuGet package to help query DynamoDB in a clean and generic way.  

## Features

### Pagination
The AWS DynamoDB SDK gives you a very limited way to paginate, while giving you a max query size of 5 MB.  
The pagination feature provides you with the expected pagination result minus the total count, as it's a NoSQL db and not intended.

### Querying secondary local indexes
Generally to query on secondary local indexes takes a fully defined QueryRequest, which seems silly for something as simple as getting a component by its Id.
This package implements methods that will populate and run the query and respond with a simple element.

## Terminology
**Entity:** a class representing a DynamoDB row, including DynamoDB attributes