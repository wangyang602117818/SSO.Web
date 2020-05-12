import React from 'react';
import { NavBar, Icon, Popover, List, PullToRefresh, Toast } from 'antd-mobile';
import { withRouter } from 'react-router-dom';
import { axios, urls } from '../config/http';
import '../css/usermanage.css'
class UserManage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            datas: [],
            filter: "",
            isEnd: false,
            refreshing: false,
            pageIndex: 1,
            pageSize: 15
        };
    }
    componentDidMount() {
        this.getData();
    }
    getData() {
        Toast.loading('Loading...', 5);
        if (this.state.isEnd) {
            Toast.info('已到达末尾', 1);
            this.setState({ refreshing: false });
            return;
        }
        axios.get(urls.user.getbasic + "?pageIndex=" + this.state.pageIndex + "&pageSize=" + this.state.pageSize + "&filter=" + this.state.filter).then(response => {
            Toast.hide();
            if (response.code === 0) {
                if (response.result.length === 0) {
                    this.setState({ isEnd: true, refreshing: false });
                    if (this.state.pageIndex > 1) Toast.info('已到达末尾', 1);
                } else {
                    var res = this.state.datas.concat(response.result);
                    this.setState({ datas: res, refreshing: false });
                }
            }
        });
    }
    itemClick(id) {
        this.props.history.push('/userupdate/' + id);
    }
    getUserList(datas) {
        var array = [];
        datas.forEach((item) => {
            array.push(
                <List.Item
                    arrow="horizontal"
                    thumb={
                        <svg className="icon icon2" aria-hidden="true">
                            <use xlinkHref="#iconpic"></use>
                        </svg>
                    }
                    multipleLine
                    onClick={() => { this.itemClick(item.UserId) }}
                    key={item._id}
                >
                    {item.UserName}
                    <List.Item.Brief>{item.CompanyName} | {item.DepartmentName}</List.Item.Brief>
                </List.Item>
            )
        });
        return array;
    }
    loadMore() {
        this.setState({
            refreshing: true,
            pageIndex: this.state.pageIndex + 1
        });
        this.getData();
    }
    search(e) {
        var code = e.nativeEvent.keyCode;
        if (code === 13) {
            //此处不设置setState改变状态
            this.state.isEnd = false;
            this.state.pageIndex = 1;
            this.state.datas = [];
            this.getData();
        }
    }
    render() {
        var dataEmpty = this.state.datas.length === 0 ? true : false;
        return (
            <div className="user_manage">
                <NavBar
                    mode="dack"
                    icon={<Icon type="left" />}
                    onLeftClick={() => { this.props.history.goBack(-1);}}
                    rightContent={
                        <Popover mask
                            visible={this.state.visible}
                            overlay={[
                                (<Popover.Item
                                    key="1"
                                    value="add"
                                    icon=
                                    {
                                        <svg className="icon" aria-hidden="true">
                                            <use xlinkHref="#iconadd"></use>
                                        </svg>
                                    }
                                    data-seed="logId">添加
                                </Popover.Item>)
                            ]}
                            align={{
                                overflow: { adjustY: 0, adjustX: 0 },
                                offset: [-10, 0],
                            }}
                            onVisibleChange={this.handleVisibleChange}
                            onSelect={this.onSelect}
                        >
                            <div style={{
                                height: '100%',
                                padding: '0 15px',
                                marginRight: '-15px',
                                display: 'flex',
                                alignItems: 'center',
                            }}
                            >
                                <Icon type="ellipsis" />
                            </div>
                        </Popover>
                    }
                >用户管理
            </NavBar>
                <div className="search_con">
                    <input type="text"
                        name="search"
                        placeholder="Search"
                        onChange={(e) => { this.setState({ filter: e.target.value }) }}
                        onKeyPress={this.search.bind(this)}
                        defaultValue={this.state.filter} />
                </div>
                <div className="list_con">
                    <div className="no_data" style={{ display: dataEmpty && this.state.isEnd === true ? 'block' : 'none' }}>没有数据</div>
                    <List>
                        <PullToRefresh
                            damping={60}
                            direction='up'
                            refreshing={this.state.refreshing}
                            onRefresh={this.loadMore.bind(this)}
                        >
                            {this.getUserList(this.state.datas)}
                        </PullToRefresh>
                    </List>
                </div>
            </div>)
    }
}
export default withRouter(UserManage);