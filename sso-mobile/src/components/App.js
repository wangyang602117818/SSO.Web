import React from 'react';
import { TabBar } from 'antd-mobile';
import Navigator from './Navigator';
import Manage from './Manage';
import Me from './Me';
import UserManage from './UserManage'
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import '../css/app.css';

class TabBarHome extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      selectedTab: 'navigator',
      hidden: false,
      fullScreen: true
    };
  }
  render() {
    return (
      <div style={{ position: 'fixed', height: '100%', width: '100%', top: 0 }}>
        <Router>
          <Switch>
            <Route exact path="/">
              <TabBar unselectedTintColor="#949494" tintColor="#33A3F4">
                <TabBar.Item
                  title="导航"
                  key="navigator"
                  icon={
                    <svg className="icon" aria-hidden="true">
                      <use xlinkHref="#iconnavigator"></use></svg>
                  }
                  selectedIcon={
                    <svg className="icon iconblue" aria-hidden="true">
                      <use xlinkHref="#iconnavigator"></use></svg>
                  }
                  selected={this.state.selectedTab === 'navigator'}
                  onPress={() => { this.setState({ selectedTab: 'navigator' }) }}
                >
                  <Navigator />
                </TabBar.Item>
                <TabBar.Item
                  icon={
                    <svg className="icon" aria-hidden="true">
                      <use xlinkHref="#iconsetting"></use></svg>
                  }
                  selectedIcon={
                    <svg className="icon iconblue" aria-hidden="true">
                      <use xlinkHref="#iconsetting"></use></svg>
                  }
                  title="管理中心"
                  key="manage"
                  selected={this.state.selectedTab === 'manage'}
                  onPress={() => { this.setState({ selectedTab: 'manage' }) }}
                >
                  <Manage />
                </TabBar.Item>
                <TabBar.Item
                  icon={
                    <svg className="icon" aria-hidden="true">
                      <use xlinkHref="#iconme"></use></svg>
                  }
                  selectedIcon={
                    <svg className="icon iconblue" aria-hidden="true">
                      <use xlinkHref="#iconme"></use></svg>
                  }
                  title="我的"
                  key="me"
                  selected={this.state.selectedTab === "me"}
                  onPress={() => { this.setState({ selectedTab: 'me' }) }}
                >
                  <Me />
                </TabBar.Item>
              </TabBar>
            </Route>
            <Route exact path="/user">
              <UserManage />
            </Route>
          </Switch>
        </Router>
      </div>
    );
  }
}

export default TabBarHome;
