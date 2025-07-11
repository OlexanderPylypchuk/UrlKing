import UrlTable from '/src/assets/UrlTable.jsx';
import Header from '/src/assets/Header.jsx';
import { useEffect, useState } from "react";
import { useAuth } from "../contexts/AuthContext";
import UrlForm from '/src/assets/UrlForm';

function Index() {
    const { accessToken } = useAuth();
    const [refreshTrigger, setRefreshTrigger] = useState(0);

    const handleUrlAdded = () => {
        setRefreshTrigger(prev => prev + 1);
    };
    
  return (
      <>
          <Header></Header>
          {accessToken ? <UrlForm onSuccess={handleUrlAdded} />:<></>}
          <UrlTable refreshTrigger={refreshTrigger} />
      </>
  );
}

export default Index;