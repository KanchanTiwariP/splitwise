import { login } from "./services/authSerivce";

function App() {
   const handleLogin = async () => {
    try {
      const result = await login({
        email: "",
        password: "",
      });

      console.log("Login successful:", result);
    } catch (error) {
      console.error("Login failed:", error);
    }
  };

  return (
    <div>
      <h1>SplitWise</h1>
       <button onClick={handleLogin}>
        Test Login
      </button>
    </div>
  );
}

export default App
