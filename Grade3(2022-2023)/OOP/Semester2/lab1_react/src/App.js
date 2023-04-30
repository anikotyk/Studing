import './App.css';
import LoginButton from "./components/LoginButton";
import LogoutButton from "./components/LogoutButton";
import {useAuth0} from "@auth0/auth0-react";
import PageContent from "./components/PageContent";

function App() {
    const {isLoading} = useAuth0();
    if(isLoading) return <div>Loading...</div>

    return (
      <>
          <LoginButton />
          <LogoutButton />
          <PageContent />
      </>
    );
}

export default App;
