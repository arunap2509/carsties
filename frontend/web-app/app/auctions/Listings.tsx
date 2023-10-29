'use client'

import React, { useEffect, useState } from 'react'
import AuctionCard from './AuctionCard';
import AppPagination from '../components/AppPagination';
import { getData } from '../actions/auctionActions';
import Filter from './Filter';
import { useParamsStore } from '@/hooks/useParamsStore';
import { shallow } from 'zustand/shallow';
import queryString from 'query-string';
import EmptyFilter from '../components/EmptyFilter';
import { useAuctionStore } from '@/hooks/useAuctionStore';

export default function Listings() {
    const [loading, setLoading] = useState(true);
    const params = useParamsStore(state => ({
        pageSize: state.pageSize,
        pageNumber: state.pageNumber,
        searchTerm: state.searchTerm,
        orderBy: state.orderBy,
        filterBy: state.filterBy,
        seller: state.seller,
        winner: state.winner
    }), shallow)
    const data = useAuctionStore(state => ({
        auctions: state.auctions,
        pageCount: state.pageCount,
        totalCount: state.totalCount
    }), shallow);
    const setData = useAuctionStore(state => state.setData);
    const setParams = useParamsStore(state => state.setParams);
    const url = queryString.stringifyUrl({ url: '', query: params });

    const setPageNumber = (pageNumber: number) => setParams({ pageNumber });

    useEffect(() => {
        console.log("getting data");
        getData(url).then(data => {
            console.log(data);
            setData(data);
            setLoading(false);
        })
    }, [url, setData]);

    if (loading) return <h3>Loading...</h3>

    return (
        <>
            <Filter />
            {data.totalCount === 0 ? <EmptyFilter showReset /> : (
                <>
                    <div className='grid grid-cols-4 gap-6'>
                        {data.auctions && data.auctions.map((auction) => (
                            <AuctionCard key={auction.id} auction={auction} />
                        ))}
                    </div>
                    <div className='flex justify-center mt-4'>
                        <AppPagination
                            pageChanged={setPageNumber}
                            currentPage={params.pageNumber}
                            totalCount={data?.pageCount ?? 4}
                        />
                    </div>
                </>
            )}
        </>
    )
}
