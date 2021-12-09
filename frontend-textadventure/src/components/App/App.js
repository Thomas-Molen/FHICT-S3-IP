import { React} from 'react';
import { Sugar } from 'react-preloaders2';
import { withRouter } from 'react-router-dom';
import { useRecoilValue } from 'recoil';
import { useAuthHook } from '../../helpers';
import Routes from '../router';
import { PreLoaderAtom } from '../../state';
import './App.css';

function App() {
  const isLoading = useRecoilValue(PreLoaderAtom);
  useAuthHook();
  return (
    <>
      <Sugar color={'#ffffff'} background='#212529' customLoading={isLoading} time={0} />
      <Routes />
    </>
  );
}

export default withRouter(App);
