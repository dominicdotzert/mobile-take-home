# Guestlogix Take Home Test - Mobile

At Guestlogix we feel that putting developers on the spot with advanced algorithmic puzzles doesn’t exactly highlight one’s true skillset. The intention of this assessment is to see how you approach and tackle a problem in the real world, not quivering in front of a whiteboard.

### What is the test?

You will be building a mobile application to draw a route on a map between two (or more) airports. Included in this repository is a set of Airport, Airline, and Route data. Your task is to provide the user with a form to enter the origin and destination airports and display a route that connects them (if any). If a route between the origin and destination includes more than one stopover, the line drawn on the map must go through all airports in order.

### User Stories

- As a user I can enter IATA codes of an origin and a destination airport and view a path between the two on a map. Airports in the data set with a null IATA code are provided for the sake of completeness, and may be omitted.
- As a user I can enter IATA codes of an origin and destination airport that are not connected by a direct route. In this case, the route drawn on the map must show the shortest possible travel path between the two airports, going through all airports visited along the way in order. For the sake of simplicity, the shortest path is defined as the one with the least transfer (ie. it will take the same amount of time to travel between two airports, regardless of the physical distance between them). Keep in mind that an indirect route can go through more than one transfer airport before it reaches its final destination.
- As a user I am provided meaningful feedback should no route exist between the airports.
- As a user I am provided meaningful feedback if information entered is incorrect.

### Requirements

The application may be done in Xamarin or in any native language that runs on the Android or iOS platforms. Otherwise, you have complete freedom in terms of how you implement the solution, as long as all user requirements are met.

### Submitting

1. Fork this repository and provide your solution.
2. Run through it one last time to make sure it works!
3. Send an email to indicate that you have completed the challenge. 

### Questions

If you have any questions during the challenge feel free to email Peter Samsonov at psamsonov@guestlogix.com. Whether it be a question about the requirements, submitting, anything, just send the email!

# Overview of Solution

In order to meet the requirements for this challenge, my solution follows this general workflow:

1. On opening of the application, the data from each csv file is parsed and an unweighted, directed graph data structure is constructed.
2. The user is prompted to enter the IATA codes for an origin and destination airport.
3. If the request is valid, a Breadth-First-Search is performed on the graph to find the shortest path between the origin and destination. If the user's search is invalid, the user will be notified.
4. If a route is found, it is displayed on a map with a pin representing each airport and a path is traced onto the map to represent the route. If no route is found, the user will be notified.

### Graph and Search Algorithm

An adjacency list representation of a graph is used to represent the network of flights. The list of airports with valid IATA codes are the vertices of this graph and the list of routes are unweighted, directed edges. Airports in the routes list which were not found in the airports list are excluded.

To search this graph, a Breadth-First-Search is used. This algorithm is extended so that each vertex can be traced back to the origin. This is done by keeping reference of how each visited airport was reached. If the destination airport is found, it is guaranteed to be the shortest path and should immediately be returned. The collection of previous airport transfers is used to trace the destination back to the origin in order to return a list of airports from origin to destination.

### Considerations for Future Work

Although this solution does meet all the requirements, there are several considerations which I would take were I to continue to work on this problem.

1. Due to time constraints, this solution is only implemented for Android. An iOS implementation would simply require specific implementation details to the iOS project.
2. The search algorithm returns a single route. However, parallel routes of the same length are possible. Ideally this algorithm should be modified to return multiple routes if there are multiple shortest routes. This could be used to show the user all the possible options of getting to their destination.
3. Finally, additional UI improvements could be made. Airline information could be shown for each transfer between airports and more detailed information could be included for each airport. This would improve the experience for the end user and further inform them of their travel options.
