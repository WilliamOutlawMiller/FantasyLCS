// I am removing the API, as it feels unnecessary at this point. 
// The data returned from these two methods is far too big to be returned at an endpoint,
// so we might as well just stick to file IO until necessary.
UpdateData.UpdatePlayerList();
UpdateData.UpdateMatchData();