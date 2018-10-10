# RebusDemos

This demo code was used in its original form (the projects residing in the "old1" folder) at the GOTO Aarhus
conference in 2013.

A slightly updated form (the projects residing in the "old2" folder) was used in a couple of guest lectures on the
"Middleware and Communication Protocols for Dependable Systems" course at Aarhus University, and in 2017 in 
a Barcelona .NET Core meetup.

The newly updated form was used in Copenhagen .NET User Group in November 2017, and in 2018 in a guest lecture in the
"Distributed And Pervasive Systems" course at Aarhus University, at an IDA Universe presentation, and at the
Hamburg C# and .NET User Group.

The demos are:

* [Demo0](/Demo0) - demonstrates a very simple client that sends a string to a server
* [Demo1](/Demo1) - shows how a fictional Trading endpoint can publish events, which can be subscribed to by the Billing endpoint
* [Demo2](/Demo2) - introduces an external HTTP-based clearing house service, which is fairly unstable
* [Demo3](/Demo3) - introduces an external HTTP-based clearing house service, which is pretty slow
* [Demo4](/Demo4) - uses a saga to coordinate the invoicing flow
